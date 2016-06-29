using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DungeonMasterEngine.Builders.ItemInitializers;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterParser;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Player;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DoorItem = DungeonMasterParser.Items.DoorItem;
using GrabableItem = DungeonMasterEngine.DungeonContent.GrabableItems.GrabableItem;

namespace DungeonMasterEngine.Builders
{

    public class LegacyItemCreator : IItemCreator<IGrabableItem>
    {
        private readonly LegacyMapBuilder builder;
        private ItemDescriptor descriptor;

        public LegacyItemCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        public virtual IGrabableItem CreateItem(ItemData itemData)
        {
            itemData.Processed = true;
            descriptor = builder.Data.GetItemDescriptor(itemData.ObjectID.Category, ((GrabableItemData)itemData).ItemTypeIndex );

            var item = itemData.CreateItem(this);
            item.Renderer = builder.RendererSource.GetItemRenderer(item, item.Factory.Texture);
            return item;
        }


        public IEnumerable<IStorageType> GetStorageTypes(CarrryLocations locations)
        {
            Array values = Enum.GetValues(typeof(CarrryLocations));

            return values.Cast<CarrryLocations>()
                .Where(c => (c & locations) == c && c != CarrryLocations.HandsAndBackpack
                && c != CarrryLocations.Hands && c != CarrryLocations.None)
                .Select<CarrryLocations, IStorageType>(c =>
                 {
                     switch (c)
                     {
                         case CarrryLocations.Consumable:
                             return ConsumableStorageType.Instance;
                         case CarrryLocations.Head:
                             return HeadStorageType.Instance;
                         case CarrryLocations.Neck:
                             return NeckStorageType.Instance;
                         case CarrryLocations.Torso:
                             return TorsoStorageType.Instance;
                         case CarrryLocations.Legs:
                             return LegsStorageType.Instance;
                         case CarrryLocations.Feet:
                             return FeetsStorageType.Instance;
                         case CarrryLocations.Quiver1:
                             return BigQuiverStorageType.Instance;
                         case CarrryLocations.Quiver2:
                             return SmallQuiverStorageType.Instance;
                         case CarrryLocations.Pouch:
                             return PouchStorageType.Instance;
                         case CarrryLocations.Chest:
                             return ChestStorageType.Instance;
                         default:
                             throw new InvalidOperationException();
                     }
                 })
                .Concat(new IStorageType[] { ActionHandStorageType.Instance,HandStorageType.Instance, BackPackStorageType.Instance })
                .ToArray();
        }

        public IGrabableItem CreateContainer(ContainerItemData container)
        {
            container.Processed = true;
            var initializator = new ContainerInitializer
            {
                content = container
                    .GetEnumerator(builder.Data)
                    .Select(x => new LegacyItemCreator(builder).CreateItem(x))
                    .ToArray()
            };
            return builder.ContainerFactories[descriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreatePotion(PotionItemData potion)
        {
            potion.Processed = true;
            var initializator = new PotionInitializer
            {
                PotionPower = potion.PotionPower
            };
            return builder.PotionFactories[descriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreateWeapon(WeaponItemData weapon)
        {
            weapon.Processed = true;
            var initializator = new WeaponInitializer
            {
                IsBroken = weapon.IsBroken,
                ChargeCount = weapon.ChargeCount,
                IsCursed = weapon.IsCursed,
                IsPoisoned = weapon.IsPoisoned
            };
            return builder.WeaponFactories[descriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreateScrool(ScrollItemData scroll)
        {
            scroll.Processed = true;
            var initializator = new ScrollInitializer
            {
                Text = scroll.Text
            };
            return builder.ScrollFactories[descriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreateMisc(MiscellaneousItemData misc)
        {
            misc.Processed = true;
            var initializator = new MiscInitializer
            {
                Attribute = misc.AttributeValueIndex
            };
            return builder.MiscFactories[descriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreateCloth(ClothItemData cloth)
        {
            cloth.Processed = true;
            var initalizator = new ClothInitializer
            {
                IsCruised = cloth.IsCursed,
                IsBroken = cloth.IsBroken
            };

            return builder.ClothFactories[descriptor.InCategoryIndex].Create(initalizator);
        }

        public IGrabableItem CreateCreature(CreatureItem creature)
        {
            throw new InvalidOperationException("Not supported.");
        }

        public IGrabableItem CreateActuator(ActuatorItemData actuator)
        {
            throw new InvalidOperationException("Not supported.");
        }

        public IGrabableItem CreateTeleport(TeleporterItem teleport)
        {
            throw new InvalidOperationException("Not supported.");
        }

        public IGrabableItem CreateDoor(DoorItem door)
        {
            throw new InvalidOperationException("Not supported.");
        }

        public IGrabableItem CreateTextData(TextDataItem textTag)
        {
            throw new InvalidOperationException("Not supported.");
        }
    }
}
