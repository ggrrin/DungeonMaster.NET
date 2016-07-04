using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterParser;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DoorItem = DungeonMasterParser.Items.DoorItem;

namespace DungeonMasterEngine.Builders.ItemCreator
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
            return item;
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
            return builder.Factories.ContainerFactories[descriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreatePotion(PotionItemData potion)
        {
            potion.Processed = true;
            var initializator = new PotionInitializer
            {
                PotionPower = potion.PotionPower
            };
            return builder.Factories.PotionFactories[descriptor.InCategoryIndex].Create(initializator);
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
            return builder.Factories.WeaponFactories[descriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreateScrool(ScrollItemData scroll)
        {
            scroll.Processed = true;
            var initializator = new ScrollInitializer
            {
                Text = scroll.Text
            };
            return builder.Factories.ScrollFactories[descriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreateMisc(MiscellaneousItemData misc)
        {
            misc.Processed = true;
            var initializator = new MiscInitializer
            {
                Attribute = misc.AttributeValueIndex
            };
            return builder.Factories.MiscFactories[descriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreateCloth(ClothItemData cloth)
        {
            cloth.Processed = true;
            var initalizator = new ClothInitializer
            {
                IsCruised = cloth.IsCursed,
                IsBroken = cloth.IsBroken
            };

            return builder.Factories.ClothFactories[descriptor.InCategoryIndex].Create(initalizator);
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
