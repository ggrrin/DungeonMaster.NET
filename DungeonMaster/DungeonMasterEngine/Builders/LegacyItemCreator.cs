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
        public WallTileData WallTile;
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

        public Vector3 GetItemPosition(ItemData i)
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

        public WorldObject AddWallItem(ItemData i, Tile parentTile, WallTileData wallTile)
        {
            i.Processed = true;
            WallTile = wallTile;
            putOnWall = true;
            CurrentTile = parentTile;
            var res = i.CreateItem(this);
            if (res != null)
            {
                SetupGrabableItem(res, i);
                CurrentTile.SubItems.Add(res);
            }
            return res;
        }

        public WorldObject AddFloorItem(ItemData i, Tile parentTile)
        {
            i.Processed = true;
            putOnWall = false;
            CurrentTile = parentTile;
            var res = i.CreateItem(this);
            if (res != null)
            {
                SetupGrabableItem(res, i);
                CurrentTile.SubItems.Add(res);
            }
            return res;
        }

        private void SetupGrabableItem(Item item, ItemData i)
        {
            var grabable = item as GrabableItem;
            if (grabable != null)
            {
                var descriptor = builder.Data.ItemDescriptors[builder.Data.GetTableIndex(i.ObjectID.Category, (i as DungeonMasterParser.Items.GrabableItemData).ItemTypeIndex)];

                grabable.Identifer = descriptor.GlobalItemIndex;
                grabable.Name = descriptor.Name;
                grabable.TableIndex = descriptor.TableIndex;
                //TODO setup rest
            }
        }

        public Item CreateItem(ItemData itemData)
        {
            itemData.Processed = true;
            var res = itemData.CreateItem(this);

            if (res != null)
                SetupGrabableItem(res, itemData);
            return res;
        }

        public bool PrepareActuatorData(ActuatorItemData i, out Tile targetTile, out IConstrain constrain, out Texture2D decoration)
        {
            targetTile = GetTargetTile(i);
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

        Item IItemCreator<Item>.CreateContainer(ContainerItemData container)
        {
            container.Processed = true;
            return new Container(GetItemPosition(container), container.GetEnumerator(builder.Data).Select(x => (GrabableItem)CreateItem(x)).ToList());
        }

        Item IItemCreator<Item>.CreateDoor(DoorItem door)
        {
            var res = new Door(Vector3.Zero, door.HasButton);

            res.Graphic.Texture = builder.defaultDoorTexture;
            if (door.DoorAppearance)
                res.Graphic.Texture = builder.defaultMapDoorTypeTexture;

            if (door.OrnamentationID != null)
                res.Graphic.Texture = builder.DoorTextures[door.OrnamentationID.Value - 1];
            return res;
        }

        Item IItemCreator<Item>.CreatePotion(PotionItemData potion)
        {
            return new Potion(GetItemPosition(potion));
        }

        Item IItemCreator<Item>.CreateTeleport(TeleporterItem teleport)
        {
            return new Teleporter(GetItemPosition(teleport));
        }

        Item IItemCreator<Item>.CreateWeapon(WeaponItemData weapon)
        {
            return new Weapon(GetItemPosition(weapon));
        }

        Item IItemCreator<Item>.CreateTextData(TextDataItem textTag)
        {
            return new TextTag(GetItemPosition(textTag), textTag.IsVisible, textTag.TilePosition == TilePosition.East_TopRight || textTag.TilePosition == TilePosition.West_BottomRight, textTag.Text.Replace("|", Environment.NewLine));
        }

        Item IItemCreator<Item>.CreateScrool(ScrollItemData scroll)
        {
            return new Scroll(GetItemPosition(scroll));
        }

        Item IItemCreator<Item>.CreateMisc(MiscellaneousItemData misc)
        {
            return new Miscellaneous(GetItemPosition(misc));
        }

        Item IItemCreator<Item>.CreateCreature(CreatureItem creature)
        {
            return new Creature(GetItemPosition(creature));
        }

        Item IItemCreator<Item>.CreateCloth(ClothItemData cloth)
        {
            return new Cloth(GetItemPosition(cloth));
        }

        Item IItemCreator<Item>.CreateActuator(ActuatorItemData actuator)
        {
            return putOnWall ? WallActuatorCreator.ProcessWallActuator(actuator) : FloorActuatorCreator.ProcessFloorActuator(actuator);
        }

        public Tile GetTargetTile(ActuatorItemData callingActuator)
        {
            var targetPos = (callingActuator.ActLoc as RmtTrg).Position.Position.ToAbsolutePosition(builder.CurrentMap);

            Tile targetTile = null;
            if (builder.TilesPositions.TryGetValue(targetPos, out targetTile))
                return targetTile;
            else
            {
                //try find tile in raw data, and than actuator, add it to Tiles Positions
                var virtualTileData = builder.CurrentMap[targetPos.X, targetPos.Y];
                if (virtualTileData.Actuators.Count > 0)//virtual tile will be proccessed at the and so any checking shouldnt be necessary
                {
                    var newTile = new LogicTile(targetPos.ToGridVector3(builder.CurrentTile.Position.Y));
                    newTile.Gates = virtualTileData.Actuators.Where(x => x.AcutorType == 5).Select(y => InitLogicGates(y, newTile)).ToArray();//recurse
                    newTile.Counters = virtualTileData.Actuators.Where(x => x.AcutorType == 6).Select(y => InitCounters(y, newTile)).ToArray(); //recurse

                    builder.TilesPositions.Add(targetPos, targetTile = newTile);//subitems will be processed 
                }

                return targetTile; //TODO (if null)  think out what to do 
                                   //Acutor at the begining references wall near by with tag only ... what to do ? 
            }
        }

        private Counter InitCounters(ActuatorItemData gateActuator, Tile gateActuatorTile)
        {
            //if nextTarget tile is current tile do not call recurese
            Tile nextTargetTile = gateActuatorTile.GridPosition == (gateActuator.ActLoc as RmtTrg).Position.Position.ToAbsolutePosition(builder.CurrentMap) ? gateActuatorTile : GetTargetTile(gateActuator);

            return new Counter(nextTargetTile, new ActionStateX((ActionState)gateActuator.Action, (gateActuator.ActLoc as RmtTrg).Direction), gateActuator.Data, gateActuatorTile.Position);
        }

        private LogicGate InitLogicGates(ActuatorItemData gateActuator, Tile gateActuatorTile)
        {
            //if nextTarget tile is current tile do not call recurese
            Tile nextTargetTile = gateActuatorTile.GridPosition == (gateActuator.ActLoc as RmtTrg).Position.Position.ToAbsolutePosition(builder.CurrentMap) ? gateActuatorTile : GetTargetTile(gateActuator);

            return new LogicGate(nextTargetTile, new ActionStateX((ActionState)gateActuator.Action, (gateActuator.ActLoc as RmtTrg).Direction), gateActuatorTile.Position, (gateActuator.Data & 0x10) == 0x10, (gateActuator.Data & 0x20) == 0x20,
                (gateActuator.Data & 0x40) == 0x40, (gateActuator.Data & 0x80) == 0x80);
        }
    }
}
