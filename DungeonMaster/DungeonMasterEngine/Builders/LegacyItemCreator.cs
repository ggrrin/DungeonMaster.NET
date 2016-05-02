using System;
using System.Linq;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.DungeonContent.Tiles;
using System.Diagnostics;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DungeonMasterParser.Tiles;
using GrabableItem = DungeonMasterEngine.DungeonContent.Items.GrabableItem;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    public class LegacyItemCreator : IItemCreator<GrabableItem>
    {
        private readonly LegacyMapBuilder builder;
        
        public Tile CurrentTile { get; private set; }

        public LegacyItemCreator(LegacyMapBuilder builder )
        {
            this.builder = builder;
        }

        public GrabableItem CreateItem(ItemData itemData, Tile parentTile)
        {
            CurrentTile = parentTile;
            itemData.Processed = true;
            var res = itemData.CreateItem(this);

            if (res != null)
                SetupGrabableItem(res, itemData);
            return res;
        }

        private void SetupGrabableItem(Item item, ItemData i)
        {
            var grabable = item as GrabableItem;
            if (grabable != null)
            {
                var descriptor = builder.Data.ItemDescriptors[builder.Data.GetTableIndex(i.ObjectID.Category, ((GrabableItemData) i).ItemTypeIndex)];

                grabable.Identifer = descriptor.GlobalItemIndex;
                grabable.Name = descriptor.Name;
                grabable.TableIndex = descriptor.TableIndex;
                //TODO setup rest
            }
        }

        public GrabableItem CreateContainer(ContainerItemData container)
        {
            container.Processed = true;
            return new Container(builder.GetFloorPosition(container.TilePosition, CurrentTile ), container.GetEnumerator(builder.Data).Select(x => CreateItem(x, CurrentTile)).ToList());
        }

        public GrabableItem CreatePotion(PotionItemData potion)
        {
            return new Potion(builder.GetFloorPosition(potion.TilePosition, CurrentTile ));
        }

        public GrabableItem CreateWeapon(WeaponItemData weapon)
        {
            return new Weapon(builder.GetFloorPosition(weapon.TilePosition, CurrentTile ));
        }

        public GrabableItem CreateScrool(ScrollItemData scroll)
        {
            return new Scroll(builder.GetFloorPosition(scroll.TilePosition, CurrentTile ), scroll.Text);
        }

        public GrabableItem CreateMisc(MiscellaneousItemData misc)
        {
            return new Miscellaneous(builder.GetFloorPosition(misc.TilePosition, CurrentTile ));
        }

        public GrabableItem CreateCloth(ClothItemData cloth)
        {
            return new Cloth(builder.GetFloorPosition(cloth.TilePosition, CurrentTile ));
        }

        public GrabableItem CreateCreature(CreatureItem creature)
        {
            throw new InvalidOperationException("Not supported.");
        }

        public GrabableItem CreateActuator(ActuatorItemData actuator)
        {
            throw new InvalidOperationException("Not supported.");
        }

        public GrabableItem CreateTeleport(TeleporterItem teleport)
        {
            throw new InvalidOperationException("Not supported.");
        }

        public GrabableItem CreateDoor(DoorItem door)
        {
            throw new InvalidOperationException("Not supported.");
        }

        public GrabableItem CreateTextData(TextDataItem textTag)
        {
            throw new InvalidOperationException("Not supported.");
        }
    }
}
