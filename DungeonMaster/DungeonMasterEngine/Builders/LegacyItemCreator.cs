using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterParser;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Player;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using GrabableItem = DungeonMasterEngine.DungeonContent.Items.GrabableItem;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    public class LegacyItemCreator : IItemCreator<IGrabableItem>
    {
        private readonly LegacyMapBuilder builder;
        
        public Tile CurrentTile { get; private set; }

        public LegacyItemCreator(LegacyMapBuilder builder )
        {
            this.builder = builder;
        }

        public IGrabableItem CreateItem(ItemData itemData, Tile parentTile)
        {
            CurrentTile = parentTile;
            itemData.Processed = true;
            var res = itemData.CreateItem(this);

            if (res != null)
                SetupGrabableItem(res, itemData);
            return res;
        }

        private void SetupGrabableItem(IGrabableItem item, ItemData i)
        {
            var descriptor = builder.Data.ItemDescriptors[builder.Data.GetTableIndex(i.ObjectID.Category, ((GrabableItemData) i).ItemTypeIndex)];
            item.Identifer = descriptor.GlobalItemIndex;
            item.Name = descriptor.Name;
            item.TableIndex = descriptor.TableIndex;
            item.PossibleStorages = GetStorageTypes(descriptor.CarryLocation);
        }

        public IEnumerable<IStorageType> GetStorageTypes(CarrryLocations locations)
        {
            Array values = Enum.GetValues(typeof(CarrryLocations));

            return values.Cast<CarrryLocations>()
                .Where(c => (c & locations) == c && c != CarrryLocations.HandsAndBackpack
                && c != CarrryLocations.Hands && c != CarrryLocations.None)
                .Select<CarrryLocations,IStorageType>(c =>
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
                .Concat(new IStorageType[] { HandStorageType.Instance, BackPackStorageType.Instance})
                .ToArray();
        }

        public IGrabableItem CreateContainer(ContainerItemData container)
        {
            container.Processed = true;
            return new Container(builder.GetFloorPosition(container.TilePosition, CurrentTile), container.GetEnumerator(builder.Data).Select(x => CreateItem(x, CurrentTile)).ToList());
        }

        public IGrabableItem CreatePotion(PotionItemData potion)
        {
            return new Potion(builder.GetFloorPosition(potion.TilePosition, CurrentTile));
        }

        public IGrabableItem CreateWeapon(WeaponItemData weapon)
        {
            return new Weapon(builder.GetFloorPosition(weapon.TilePosition, CurrentTile));
        }

        public IGrabableItem CreateScrool(ScrollItemData scroll)
        {
            return new Scroll(builder.GetFloorPosition(scroll.TilePosition, CurrentTile), scroll.Text);
        }

        public IGrabableItem CreateMisc(MiscellaneousItemData misc)
        {
            return new Miscellaneous(builder.GetFloorPosition(misc.TilePosition, CurrentTile));
        }

        public IGrabableItem CreateCloth(ClothItemData cloth)
        {
            return new Cloth(builder.GetFloorPosition(cloth.TilePosition, CurrentTile));
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
