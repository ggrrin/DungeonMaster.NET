using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.Actuators;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using DungeonMasterParser.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrabableItem = DungeonMasterEngine.DungeonContent.Items.GrabableItem;
using Tile = DungeonMasterEngine.DungeonContent.Tiles.Tile;

namespace DungeonMasterEngine.Builders
{
    public class WallActuatorCreator
    {
        private readonly LegacyMapBuilder builder;
        
        private LegacyItemCreator itemCreator => builder.LegacyItemCreator;
        public Tile CurrentTile => itemCreator.CurrentTile;

        public WallTile wallTile => itemCreator.WallTile;
        
        public WallActuatorCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        public Item ProcessWallActuator(ActuatorItem i)
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
                    var ch = new ChampoinActuator(itemCreator.GetItemPosition(i), new ChampoinBuilder(builder, wallTile, CurrentTile).GetChampoin(i));
                    (ch.Graphics as CubeGraphic).Texture = builder.WallTextures[i.Decoration - 1];
                    return ch;

                default:
                    //throw new InvalidOperationException("Unsupportted actuator type.");
                    Point? absolutePosition = null;
                    if (i.ActLoc is RmtTrg)
                        absolutePosition = (i.ActLoc as RmtTrg).Position.Position.ToAbsolutePosition(builder.CurrentMap);

                    return new Actuator(itemCreator.GetItemPosition(i), $"{absolutePosition} {i.DumpString()}");

            }
        }

        private Item GetKeyHole3a4(ActuatorItem i, bool destroyItem)
        {
            Texture2D decoration;
            IConstrain constrain;
            Tile targetTile;
            itemCreator.PrepareActuatorData(i, out targetTile, out constrain, out decoration);
            var res = new KeyHoleActuator(itemCreator.GetItemPosition(i), targetTile, new ActionStateX((ActionState)i.Action, (i.ActLoc as RmtTrg).Direction), constrain, destroyItem);
            res.DecorationTexture = decoration;
            return res;
        }

        private Item GetSwitch1(ActuatorItem i)
        {
            if (i.IsLocal)
            {
                if (((LocTrg)i.ActLoc).RotateAutors)
                {
                    var alcoveSuperItem = wallTile.Actuators.Find(x => !x.Processed);
                    if (alcoveSuperItem == null || alcoveSuperItem.AcutorType != 0)
                        throw new NotSupportedException("yet");

                    var alcove = itemCreator.GetItem(alcoveSuperItem) as AlcoveActuator;

                    alcove.Hidden = true;
                    alcove.HideoutTexture = builder.WallTextures[i.Decoration - 1];
                    return alcove;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else//lever //TODO
            {
                var leverDown = wallTile.Actuators.Find(x => !x.Processed);
                if (leverDown != null && leverDown.AcutorType == 1)
                {
                    leverDown.Processed = true;
                    return new LeverActuator(itemCreator.GetItemPosition(i), GetTargetTile(i),
                        leverDown.IsRevertable, new ActionStateX((ActionState)i.Action,
                        ((RmtTrg)i.ActLoc).Direction),
                        (i.ActLoc as RmtTrg)?.Position.Direction ?? Direction.North)
                    {
                        UpTexture = builder.WallTextures[i.Decoration - 1],
                        DownTexture = builder.WallTextures[leverDown.Decoration - 1]
                    };
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public Tile GetTargetTile(ActuatorItem callingActuator)
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

        private Counter InitCounters(ActuatorItem gateActuator, Tile gateActuatorTile)
        {
            //if nextTarget tile is current tile do not call recurese
            Tile nextTargetTile = gateActuatorTile.GridPosition == (gateActuator.ActLoc as RmtTrg).Position.Position.ToAbsolutePosition(builder.CurrentMap) ? gateActuatorTile : GetTargetTile(gateActuator);

            return new Counter(nextTargetTile, new ActionStateX((ActionState)gateActuator.Action, (gateActuator.ActLoc as RmtTrg).Direction), gateActuator.Data, gateActuatorTile.Position);
        }

        private LogicGate InitLogicGates(ActuatorItem gateActuator, Tile gateActuatorTile)
        {
            //if nextTarget tile is current tile do not call recurese
            Tile nextTargetTile = gateActuatorTile.GridPosition == (gateActuator.ActLoc as RmtTrg).Position.Position.ToAbsolutePosition(builder.CurrentMap) ? gateActuatorTile : GetTargetTile(gateActuator);

            return new LogicGate(nextTargetTile, new ActionStateX((ActionState)gateActuator.Action, (gateActuator.ActLoc as RmtTrg).Direction), gateActuatorTile.Position, (gateActuator.Data & 0x10) == 0x10, (gateActuator.Data & 0x20) == 0x20,
                (gateActuator.Data & 0x40) == 0x40, (gateActuator.Data & 0x80) == 0x80);
        }

        private Item GetAlcove(ActuatorItem i)
        {
            if (!i.IsLocal) //only decoration
            {
                return new DecorationItem(itemCreator.GetItemPosition(i), builder.WallTextures[i.Decoration - 1]);
            }
            else//alcove
            {
                var items = wallTile.Items
                    .OfType<DungeonMasterParser.Items.GrabableItem>()
                    .Where(x => !x.Processed)
                    .Select(x => (GrabableItem)itemCreator.GetItem(x));

                return new AlcoveActuator(itemCreator.GetItemPosition(i), items) { AlcoveTexture = builder.WallTextures[i.Decoration - 1] };
            }
        }

        private Actuator GetExchanger(ActuatorItem i)
        {
            if (i.IsLocal)
            {
                ; //TODO find out what to do (mancacles in the first level points to its wall tile)   

                var constrain = new GrabableItemConstrain(i.Data, acceptOthers: false);
                var item = (from k in wallTile.Items where k is DungeonMasterParser.Items.GrabableItem select new LegacyItemCreator(builder).GetItem(k)).FirstOrDefault();

                //TODO select appropriate items
                var res = new ExchangerActuator(itemCreator.GetItemPosition(i), (GrabableItem)item, constrain);
                res.DecorationActivated = builder.WallTextures[i.Decoration - 1];

                if ((i.ActLoc as LocTrg).RotateAutors)
                {
                    var inactivatActuator = wallTile.Actuators.Find(x => x.AcutorType == 0);
                    inactivatActuator.Processed = true;
                    res.DecorationDeactived = builder.WallTextures[inactivatActuator.Decoration - 1];
                }
                res.UpdateDecoration();
                return res;
            }
            else
                throw new NotSupportedException();
        }
    }
}
