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
using DungeonMasterParser.Items.@abstract;
using DungeonMasterParser.Tiles;
using GrabableItem = DungeonMasterEngine.DungeonContent.Items.GrabableItem;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    public partial class OldDungeonBuilder : IDungonBuilder
    {
        class ItemCreator : IItemCreator<Item>
        {
            private OldDungeonBuilder builder;
            private Tile currentTile;
            private WallTile wallTile;
            private bool putOnWall;

            public ItemCreator(OldDungeonBuilder builder)
            {
                this.builder = builder;
            }

            private Vector3 GetItemPosition(SuperItem i)
            {
                if (currentTile == null)
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

                return currentTile.Position + offset;
            }

            public WorldObject AddWallItem(SuperItem i, Tile parentTile, WallTile wallTile)
            {
                i.Processed = true;
                this.wallTile = wallTile;
                putOnWall = true;
                currentTile = parentTile;
                var res = i.GetItem(this);
                if (res != null)
                {
                    SetupItem(res, i);
                    currentTile.SubItems.Add(res);
                }
                return res;
            }

            public WorldObject AddFloorItem(SuperItem i, Tile parentTile)
            {
                i.Processed = true;
                putOnWall = false;
                currentTile = parentTile;
                var res = i.GetItem(this);
                if (res != null)
                {
                    SetupItem(res, i);
                    currentTile.SubItems.Add(res);
                }
                return res;
            }

            public Item GetItem(SuperItem i)
            {
                i.Processed = true;
                var res = i.GetItem(this);

                if (res != null)
                    SetupItem(res, i);
                return res;
            }

            Item IItemCreator<Item>.GetItem(ContainerItem i)
            {
                i.Processed = true;
                return new Container(GetItemPosition(i), i.GetEnumerator(builder.data).Select(x => (GrabableItem)GetItem(x)).ToList());
            }

            private void SetupItem(Item item, SuperItem i)
            {
                var grabable = item as GrabableItem;
                if (grabable != null)
                {
                    var descriptor = builder.data.ItemDescriptors[builder.data.GetTableIndex(i.ObjectID.Category, (i as DungeonMasterParser.Items.@abstract.GrabableItem).ItemTypeIndex)];

                    grabable.Identifer = descriptor.GlobalItemIndex;
                    grabable.Name = descriptor.Name;
                    grabable.TableIndex = descriptor.TableIndex;
                    //TODO setup rest
                }
            }


            Item IItemCreator<Item>.GetItem(DoorItem i)
            {
                var res = new Door(Vector3.Zero, i.HasButton);

                res.Graphic.Texture = builder.defaultDoorTexture;
                if (i.DoorAppearance)
                    res.Graphic.Texture = builder.defaultMapDoorTypeTexture;

                if (i.OrnamentationID != null)
                    res.Graphic.Texture = builder.doorTextures[i.OrnamentationID.Value - 1];
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
                if (putOnWall)
                    return ProcessWallActuator(i);
                else
                    return ProcessFloorActuator(i);
            }

            #region floor

            private Item ProcessFloorActuator(ActuatorItem i)
            {
                switch (i.AcutorType)
                {
                    case 0:
                        return new DecorationItem(GetItemPosition(i), builder.floorTextures[i.Decoration - 1]);
                    case 1:
                        return Actuator1(i);
                    case 2:
                        return Actuator2(i);
                    case 3:
                        return PartyActuator3(i);
                    case 4:
                        return Actuator4(i);
                    case 7:
                        return Actuator7(i);
                    case 8:
                        return FloorActuatorType8(i);

                }


                Point? absolutePosition = null;
                if (i.ActionLocation is RemoteTarget)
                    absolutePosition = (i.ActionLocation as RemoteTarget).Position.Position.ToAbsolutePosition(builder.map);

                return new Actuator(GetItemPosition(i), $"{absolutePosition} {i.DumpString()}");
            }



            private Actuator Actuator7(ActuatorItem i)
            {
                IConstrain constrain = null;
                Tile targetTile = null;
                Texture2D decoration = null;
                PrepareActuatorData(i, out targetTile, out constrain, out decoration);

                var res = new CreatureActuator(GetItemPosition(i), currentTile, targetTile, new ActionStateX((ActionState)i.Action));
                res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
                return res;
            }

            private Actuator Actuator4(ActuatorItem i)
            {
                IConstrain constrain = null;
                Tile targetTile = null;
                Texture2D decoration = null;
                PrepareActuatorData(i, out targetTile, out constrain, out decoration);

                var res = new ItemActuator(GetItemPosition(i), currentTile, targetTile, constrain, new ActionStateX((ActionState)i.Action));
                res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
                return res;
            }

            private Actuator Actuator1(ActuatorItem i)
            {
                IConstrain constrain = null;
                Tile targetTile = null;
                Texture2D decoration = null;
                PrepareActuatorData(i, out targetTile, out constrain, out decoration);

                var res = new TheronPartyCreatureItemActuator(GetItemPosition(i), currentTile, targetTile, new ActionStateX((ActionState)i.Action));
                //TODO 14 28 actuator sends Clear message to pit(which open the pit => should be close) 
                res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
                return res;
            }

            private Actuator Actuator2(ActuatorItem i)
            {
                IConstrain constrain = null;
                Tile targetTile = null;
                Texture2D decoration = null;
                PrepareActuatorData(i, out targetTile, out constrain, out decoration);

                var res = new TheronPartyCreatureActuator(GetItemPosition(i), currentTile, targetTile, new ActionStateX((ActionState)i.Action));
                res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
                return res;
            }

            private Actuator PartyActuator3(ActuatorItem i)
            {
                IConstrain constrain = null;
                Tile targetTile = null;
                Texture2D decoration = null;
                PrepareActuatorData(i, out targetTile, out constrain, out decoration);

                var res = new DirectionPartyActuator(GetItemPosition(i), currentTile, targetTile, new ActionStateX((ActionState)i.Action));
                res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
                return res;
            }

            private Actuator FloorActuatorType8(ActuatorItem i)
            {  //TODO use PrepareActuatorData
                if (!i.IsLocal)
                {
                    Tile remoteTile = null;
                    if (builder.tilesPositions.TryGetValue((i.ActionLocation as RemoteTarget).Position.Position.ToAbsolutePosition(builder.map), out remoteTile))
                    {
                        IConstrain constrain = null;
                        if (i.Data > 0)
                            constrain = new GrabableItemConstrain(i.Data, acceptOthers: false);
                        else
                            constrain = new NoConstrain();
                        return new ItemPartyActuator(GetItemPosition(i), currentTile, remoteTile, constrain, new ActionStateX((ActionState)i.Action));

                    }
                    else
                        throw new NotSupportedException("yet");
                }
                else
                    throw new NotSupportedException("yet");
            }

            #endregion

            private bool PrepareActuatorData(ActuatorItem i, out Tile targetTile, out IConstrain constrain, out Texture2D decoration)
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
                {
                    if (putOnWall)
                        decoration = builder.wallTextures[i.Decoration - 1];
                    else
                        decoration = builder.floorTextures[i.Decoration - 1];
                }

                return true;
            }

            private Item ProcessWallActuator(ActuatorItem i)
            {
                switch (i.AcutorType)
                {
                    case 0:
                        return GetAlcove(i);

                    case 1:
                        return GetSwitch1(i);

                    case 3:
                        return GetKeyHole3a4(i, destroyItem: false);

                    case 4:
                        return GetKeyHole3a4(i, destroyItem: true);

                    //case 5:
                    //acutor of this type are created when needed by another actuators
                    //break;

                    //case 6:
                    //acutor of this type are created when needed by another actuators
                    //break;

                    case 13:
                        return GetExchanger(i);

                    case 127:
                        var ch = new ChampoinActuator(GetItemPosition(i), new ChampoinBuilder(builder, wallTile, currentTile).GetChampoin(i));
                        (ch.Graphics as CubeGraphic).Texture = builder.wallTextures[i.Decoration - 1];
                        return ch;

                    default:
                        //throw new InvalidOperationException("Unsupportted actuator type.");
                        Point? absolutePosition = null;
                        if (i.ActionLocation is RemoteTarget)
                            absolutePosition = (i.ActionLocation as RemoteTarget).Position.Position.ToAbsolutePosition(builder.map);

                        return new Actuator(GetItemPosition(i), $"{absolutePosition} {i.DumpString()}");

                }
            }


            private Item GetKeyHole3a4(ActuatorItem i, bool destroyItem)
            {
                Texture2D decoration;
                IConstrain constrain;
                Tile targetTile;
                PrepareActuatorData(i, out targetTile, out constrain, out decoration);
                var res = new KeyHoleActuator(GetItemPosition(i), targetTile, new ActionStateX((ActionState)i.Action, (i.ActionLocation as RemoteTarget).Direction), constrain, destroyItem);
                res.DecorationTexture = decoration;
                return res;
            }

            private Item GetSwitch1(ActuatorItem i)
            {
                if (i.IsLocal && (i.ActionLocation as LocalTarget).RotateAutors)
                {
                    var alcoveSuperItem = wallTile.Actuators.Find(x => !x.Processed);

                    if (alcoveSuperItem == null || alcoveSuperItem.AcutorType != 0)
                        return null; //throw new NotSupportedException("yet"); //TODO

                    var alcove = GetItem(alcoveSuperItem) as AlcoveActuator;

                    alcove.Hidden = true;
                    alcove.HideoutTexture = builder.wallTextures[i.Decoration - 1];
                    return alcove;
                }
                else//lever //TODO
                {
                    var leverDown = wallTile.Actuators.Find(x => !x.Processed);
                    if (leverDown != null && leverDown.AcutorType == 1)
                    {
                        var res = new LeverActuator(GetItemPosition(i), GetTargetTile(i),
                            /*!((LocalTarget)leverDown.ActionLocation).RotateAutors*/
                            leverDown.IsRevertable,
                            new ActionStateX((ActionState)i.Action, (i.ActionLocation as RemoteTarget).Direction),
                            (i.ActionLocation as RemoteTarget)?.Position.Direction ?? Direction.North);

                        res.UpTexture = builder.wallTextures[i.Decoration - 1];
                        res.DownTexture = builder.wallTextures[leverDown.Decoration - 1];
                        return res;
                    }

                    return null;
                }
            }

            private Tile GetTargetTile(ActuatorItem callingActuator)
            {
                var targetPos = (callingActuator.ActionLocation as RemoteTarget).Position.Position.ToAbsolutePosition(builder.map);

                Tile targetTile = null;
                if (builder.tilesPositions.TryGetValue(targetPos, out targetTile))
                    return targetTile;
                else
                {
                    //try find tile in raw data, and than actuator, add it to Tiles Positions
                    var virtualTileData = builder.map[targetPos.X, targetPos.Y];
                    if (virtualTileData.Actuators.Count > 0)//virtual tile will be proccessed at the and so any checking shouldnt be necessary
                    {
                        var newTile = new LogicTile(targetPos.ToGridVector3(builder.currentTile.Position.Y));
                        newTile.Gates = virtualTileData.Actuators.Where(x => x.AcutorType == 5).Select(y => InitLogicGates(y, newTile)).ToArray();//recurse
                        newTile.Counters = virtualTileData.Actuators.Where(x => x.AcutorType == 6).Select(y => InitCounters(y, newTile)).ToArray(); //recurse

                        builder.tilesPositions.Add(targetPos, targetTile = newTile);//subitems will be processed 
                    }

                    return targetTile; //TODO (if null)  think out what to do 
                                       //Acutor at the begining references wall near by with tag only ... what to do ? 
                }
            }

            private Counter InitCounters(ActuatorItem gateActuator, Tile gateActuatorTile)
            {
                //if nextTarget tile is current tile do not call recurese
                Tile nextTargetTile = gateActuatorTile.GridPosition == (gateActuator.ActionLocation as RemoteTarget).Position.Position.ToAbsolutePosition(builder.map) ? gateActuatorTile : GetTargetTile(gateActuator);

                return new Counter(nextTargetTile, new ActionStateX((ActionState)gateActuator.Action, (gateActuator.ActionLocation as RemoteTarget).Direction), gateActuator.Data, gateActuatorTile.Position);
            }

            private LogicGate InitLogicGates(ActuatorItem gateActuator, Tile gateActuatorTile)
            {
                //if nextTarget tile is current tile do not call recurese
                Tile nextTargetTile = gateActuatorTile.GridPosition == (gateActuator.ActionLocation as RemoteTarget).Position.Position.ToAbsolutePosition(builder.map) ? gateActuatorTile : GetTargetTile(gateActuator);

                return new LogicGate(nextTargetTile, new ActionStateX((ActionState)gateActuator.Action, (gateActuator.ActionLocation as RemoteTarget).Direction), gateActuatorTile.Position, (gateActuator.Data & 0x10) == 0x10, (gateActuator.Data & 0x20) == 0x20,
                    (gateActuator.Data & 0x40) == 0x40, (gateActuator.Data & 0x80) == 0x80);
            }

            private Item GetAlcove(ActuatorItem i)
            {
                if (!i.IsLocal) //only decoration
                {
                    return new DecorationItem(GetItemPosition(i), builder.wallTextures[i.Decoration - 1]);
                }
                else//alcove
                {
                    //var item = (from k in wallTile.Items where k is DungeonMasterParser.GrabableItem select new ItemCreator(builder).GetItem(k)).FirstOrDefault();

                    //var res = new ExchangerActuator(GetItemPosition(i), item  as Items.GrabableItem, new TypeConstrain(typeof(Items.GrabableItem)));
                    //res.DecorationActivated = res.DecorationDeactived = builder.wallTextures[i.Decoration - 1];
                    //res.UpdateDecoration();
                    //return res;


                    var items = from x in wallTile.Items
                                where !x.Processed && x.TilePosition == i.TilePosition && x is DungeonMasterParser.Items.@abstract.GrabableItem
                                select (GrabableItem)GetItem(x);

                    var res = new AlcoveActuator(GetItemPosition(i), items);
                    res.AlcoveTexture = builder.wallTextures[i.Decoration - 1];
                    return res;
                }

            }

            private Actuator GetExchanger(ActuatorItem i)
            {
                if (i.IsLocal)
                {
                    ; //TODO find out what to do (mancacles in the first level points to its wall tile)   

                    var constrain = new GrabableItemConstrain(i.Data, acceptOthers: false);
                    var item = (from k in wallTile.Items where k is DungeonMasterParser.Items.@abstract.GrabableItem select new ItemCreator(builder).GetItem(k)).FirstOrDefault();

                    //TODO select appropriate items
                    var res = new ExchangerActuator(GetItemPosition(i), (GrabableItem)item, constrain);
                    res.DecorationActivated = builder.wallTextures[i.Decoration - 1];

                    if ((i.ActionLocation as LocalTarget).RotateAutors)
                    {
                        var inactivatActuator = wallTile.Actuators.Find(x => x.AcutorType == 0);
                        inactivatActuator.Processed = true;
                        res.DecorationDeactived = builder.wallTextures[inactivatActuator.Decoration - 1];
                    }
                    res.UpdateDecoration();
                    return res;
                }
                else
                    throw new NotSupportedException();
            }
        }
    }
}
