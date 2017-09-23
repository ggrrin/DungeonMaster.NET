using System;
using System.Linq;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterParser;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DoorItem = DungeonMasterParser.Items.DoorItem;

namespace DungeonMasterEngine.Builders.ItemCreators
{
    public interface ICreator
    {
        void Reset();
    }

    public interface ILegacyItemCreator : ICreator
    {
        IGrabableItem CreateItem(ItemData itemData);
    }


    public class LegacyItemCreator : ILegacyItemCreator, IItemCreator<IGrabableItem>
    {
        protected readonly LegacyMapBuilder builder;
        protected ItemDescriptor currentDescriptor;

        public LegacyItemCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        public void Reset()
        {
            currentDescriptor = null;
        }


        public virtual IGrabableItem CreateItem(ItemData itemData)
        {
            itemData.Processed = true;
            currentDescriptor = builder.Data.GetItemDescriptor(itemData.ObjectID.Category, ((GrabableItemData)itemData).ItemTypeIndex);

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
                    .Select(x =>
                    {
                        var savedDescriptor = currentDescriptor;
                        var res = CreateItem(x);//overrides currentDescriptor variable
                        currentDescriptor = savedDescriptor;
                        return res;
                    })
                    .ToArray()
            };
            return builder.Factories.ContainerFactories[currentDescriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreatePotion(PotionItemData potion)
        {
            potion.Processed = true;
            var initializator = new PotionInitializer
            {
                PotionPower = potion.PotionPower
            };
            return builder.Factories.PotionFactories[currentDescriptor.InCategoryIndex].Create(initializator);
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
            return builder.Factories.WeaponFactories[currentDescriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreateScrool(ScrollItemData scroll)
        {
            scroll.Processed = true;
            var initializator = new ScrollInitializer
            {
                Text = scroll.Text
            };
            return builder.Factories.ScrollFactories[currentDescriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreateMisc(MiscellaneousItemData misc)
        {
            misc.Processed = true;
            var initializator = new MiscInitializer
            {
                Attribute = misc.AttributeValueIndex
            };
            return builder.Factories.MiscFactories[currentDescriptor.InCategoryIndex].Create(initializator);
        }

        public IGrabableItem CreateCloth(ClothItemData cloth)
        {
            cloth.Processed = true;
            var initalizator = new ClothInitializer
            {
                IsCruised = cloth.IsCursed,
                IsBroken = cloth.IsBroken
            };

            return builder.Factories.ClothFactories[currentDescriptor.InCategoryIndex].Create(initalizator);
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
