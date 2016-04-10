using System;
using System.Linq;
using DungeonMasterParser;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.DungeonContent.Items.Actuators;
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
    public class LegacyItemCreator : IItemCreator<Item>
    {
        private readonly LegacyMapBuilder builder;
        public WallTile WallTile;
        private bool putOnWall;
        
        public Tile CurrentTile { get; private set; }
        public FloorActuatorCreator FloorActuatorCreator { get; } 
        public WallActuatorCreator WallActuatorCreator { get; } 

        public LegacyItemCreator(LegacyMapBuilder builder )
        {
            this.builder = builder;
            WallActuatorCreator = new WallActuatorCreator(builder);
            FloorActuatorCreator = new FloorActuatorCreator(builder);
        }

        public Vector3 GetItemPosition(SuperItem i)
        {
            if (CurrentTile == null)
                return Vector3.Zero;

            Vector3 offset = Vector3.Zero;

            const float scalarOffset = 0.25f;

            if (!putOnWall)
            {
                switch (i.TilePosition)
                {
                    case TilePosition.North_TopLeft:
                        offset = new Vector3(scalarOffset, 0, scalarOffset);
                        break;
                    case TilePosition.East_TopRight:
                        offset = new Vector3(1 - scalarOffset, 0, scalarOffset);
                        break;
                    case TilePosition.South_BottomLeft:
                        offset = new Vector3(scalarOffset, 0, 1 - scalarOffset);
                        break;
                    case TilePosition.West_BottomRight:
                        offset = new Vector3(1 - scalarOffset, 0, 1 - scalarOffset);
                        break;
                }
            }
            else
            {
                switch (i.TilePosition)
                {
                    case TilePosition.North_TopLeft:
                        offset = new Vector3(scalarOffset, scalarOffset, 1 - scalarOffset);
                        break;
                    case TilePosition.East_TopRight:
                        offset = new Vector3(0, scalarOffset, 1 - scalarOffset);
                        break;
                    case TilePosition.South_BottomLeft:
                        offset = new Vector3(scalarOffset, scalarOffset, 0);
                        break;
                    case TilePosition.West_BottomRight:
                        offset = new Vector3(1 - scalarOffset, scalarOffset, scalarOffset);
                        break;
                }
            }

            return CurrentTile.Position + offset;
        }

        public WorldObject AddWallItem(SuperItem i, Tile parentTile, WallTile wallTile)
        {
            i.Processed = true;
            WallTile = wallTile;
            putOnWall = true;
            CurrentTile = parentTile;
            var res = i.GetItem(this);
            if (res != null)
            {
                SetupGrabableItem(res, i);
                CurrentTile.SubItems.Add(res);
            }
            return res;
        }

        public WorldObject AddFloorItem(SuperItem i, Tile parentTile)
        {
            i.Processed = true;
            putOnWall = false;
            CurrentTile = parentTile;
            var res = i.GetItem(this);
            if (res != null)
            {
                SetupGrabableItem(res, i);
                CurrentTile.SubItems.Add(res);
            }
            return res;
        }

        private void SetupGrabableItem(Item item, SuperItem i)
        {
            var grabable = item as GrabableItem;
            if (grabable != null)
            {
                var descriptor = builder.Data.ItemDescriptors[builder.Data.GetTableIndex(i.ObjectID.Category, (i as DungeonMasterParser.Items.GrabableItem).ItemTypeIndex)];

                grabable.Identifer = descriptor.GlobalItemIndex;
                grabable.Name = descriptor.Name;
                grabable.TableIndex = descriptor.TableIndex;
                //TODO setup rest
            }
        }

        public Item GetItem(SuperItem i)
        {
            i.Processed = true;
            var res = i.GetItem(this);

            if (res != null)
                SetupGrabableItem(res, i);
            return res;
        }

        public bool PrepareActuatorData(ActuatorItem i, out Tile targetTile, out IConstrain constrain, out Texture2D decoration)
        {
            targetTile = WallActuatorCreator.GetTargetTile(i);
            constrain = null;
            decoration = null;

            if (i.Data > 0)
                constrain = new GrabableItemConstrain(i.Data, acceptOthers: i.IsRevertable);
            else
                constrain = new NoConstrain();

            if (i.IsLocal)
                throw new NotSupportedException("yet");

            if (i.Decoration > 0)
                decoration = putOnWall ? builder.WallTextures[i.Decoration - 1] : builder.FloorTextures[i.Decoration - 1];

            return true;
        }

        Item IItemCreator<Item>.GetItem(ContainerItem i)
        {
            i.Processed = true;
            return new Container(GetItemPosition(i), i.GetEnumerator(builder.Data).Select(x => (GrabableItem)GetItem(x)).ToList());
        }

        Item IItemCreator<Item>.GetItem(DoorItem i)
        {
            var res = new Door(Vector3.Zero, i.HasButton);

            res.Graphic.Texture = builder.defaultDoorTexture;
            if (i.DoorAppearance)
                res.Graphic.Texture = builder.defaultMapDoorTypeTexture;

            if (i.OrnamentationID != null)
                res.Graphic.Texture = builder.DoorTextures[i.OrnamentationID.Value - 1];
            return res;
        }

        Item IItemCreator<Item>.GetItem(PotionItem i)
        {
            return new Potion(GetItemPosition(i));
        }

        Item IItemCreator<Item>.GetItem(TeleporterItem i)
        {
            return new Teleporter(GetItemPosition(i));
        }

        Item IItemCreator<Item>.GetItem(WeaponItem i)
        {
            return new Weapon(GetItemPosition(i));
        }

        Item IItemCreator<Item>.GetItem(TextDataItem i)
        {
            return new TextTag(GetItemPosition(i), i.IsVisible, i.TilePosition == TilePosition.East_TopRight || i.TilePosition == TilePosition.West_BottomRight, i.Text.Replace("|", Environment.NewLine));
        }

        Item IItemCreator<Item>.GetItem(ScrollItem i)
        {
            return new Scroll(GetItemPosition(i));
        }

        Item IItemCreator<Item>.GetItem(MiscellaneousItem i)
        {
            return new Miscellaneous(GetItemPosition(i));
        }

        Item IItemCreator<Item>.GetItem(CreatureItem i)
        {
            return new Creature(GetItemPosition(i));
        }

        Item IItemCreator<Item>.GetItem(ClothItem i)
        {
            return new Cloth(GetItemPosition(i));
        }

        Item IItemCreator<Item>.GetItem(ActuatorItem i)
        {
            return putOnWall ? WallActuatorCreator.ProcessWallActuator(i) : FloorActuatorCreator.ProcessFloorActuator(i);
        }
    }
}
