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
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders
{
    public class FloorActuatorCreator
    {
        private readonly LegacyMapBuilder builder;

        public Tile CurrentTile => itemCreator.CurrentTile;
        public LegacyItemCreator itemCreator => builder.LegacyItemCreator;

        public FloorActuatorCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        public Item ProcessFloorActuator(ActuatorItemData i)
        {
            switch (i.AcutorType)
            {
                case 0:
                    return new DecorationItem(itemCreator.GetItemPosition(i), builder.FloorTextures[i.Decoration - 1]);
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
            if (i.ActLoc is RmtTrg)
                absolutePosition = (i.ActLoc as RmtTrg).Position.Position.ToAbsolutePosition(builder.CurrentMap);

            return new Actuator(itemCreator.GetItemPosition(i), $"{absolutePosition} {i.DumpString()}");
        }

        private Actuator Actuator7(ActuatorItemData i)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            itemCreator.PrepareActuatorData(i, out targetTile, out constrain, out decoration);

            var res = new CreatureActuator(itemCreator.GetItemPosition(i), CurrentTile, targetTile, new ActionStateX((ActionState)i.Action));
            res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
            return res;
        }

        private Actuator Actuator4(ActuatorItemData i)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            itemCreator.PrepareActuatorData(i, out targetTile, out constrain, out decoration);

            var res = new ItemActuator(itemCreator.GetItemPosition(i), CurrentTile, targetTile, constrain, new ActionStateX((ActionState)i.Action));
            res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
            return res;
        }

        private Actuator Actuator1(ActuatorItemData i)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            itemCreator.PrepareActuatorData(i, out targetTile, out constrain, out decoration);

            var res = new TheronPartyCreatureItemActuator(itemCreator.GetItemPosition(i), CurrentTile, targetTile, new ActionStateX((ActionState)i.Action));
            //TODO 14 28 actuator sends Clear message to pit(which open the pit => should be close) 
            res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
            return res;
        }

        private Actuator Actuator2(ActuatorItemData i)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            itemCreator.PrepareActuatorData(i, out targetTile, out constrain, out decoration);

            var res = new TheronPartyCreatureActuator(itemCreator.GetItemPosition(i), CurrentTile, targetTile, new ActionStateX((ActionState)i.Action));
            res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
            return res;
        }

        private Actuator PartyActuator3(ActuatorItemData i)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            itemCreator.PrepareActuatorData(i, out targetTile, out constrain, out decoration);

            var res = new DirectionPartyActuator(itemCreator.GetItemPosition(i), CurrentTile, targetTile, new ActionStateX((ActionState)i.Action));
            res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
            return res;
        }

        private Actuator FloorActuatorType8(ActuatorItemData i)
        {  //TODO use PrepareActuatorData
            if (!i.IsLocal)
            {
                Tile remoteTile = null;
                if (builder.TilesPositions.TryGetValue((i.ActLoc as RmtTrg).Position.Position.ToAbsolutePosition(builder.CurrentMap), out remoteTile))
                {
                    IConstrain constrain = null;
                    if (i.Data > 0)
                        constrain = new GrabableItemConstrain(i.Data, acceptOthers: false);
                    else
                        constrain = new NoConstrain();
                    return new ItemPartyActuator(itemCreator.GetItemPosition(i), CurrentTile, remoteTile, constrain, new ActionStateX((ActionState)i.Action));

                }
                else
                    throw new NotSupportedException("yet");
            }
            else
                throw new NotSupportedException("yet");
        }

    }
}
