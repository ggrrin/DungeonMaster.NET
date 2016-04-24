using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Floor;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Player;

namespace DungeonMasterEngine.Builders
{
    public class FloorActuatorCreator
    {
        private readonly LegacyMapBuilder builder;

        public Tile CurrentTile => itemCreator.CurrentTile;
        public LegacyItemCreator itemCreator => builder.ItemCreator;

        public FloorActuatorCreator(LegacyMapBuilder builder)
        {
            this.builder = builder;
        }

        public Item ProcessFloorActuator(ActuatorItemData i)
        {
            switch (i.ActuatorType)
            {
                case 0:
                    return new DecorationItem(builder.GetFloorPosition(i.TilePosition, CurrentTile), builder.FloorTextures[i.Decoration - 1]);
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
                absolutePosition = ((RmtTrg)i.ActLoc).Position.Position.ToAbsolutePosition(builder.CurrentMap);

            return new Actuator(builder.GetFloorPosition(i.TilePosition, CurrentTile), $"{absolutePosition} {i.DumpString()}");
        }

        private Actuator Actuator7(ActuatorItemData i)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            builder.PrepareActuatorData(i, out targetTile, out constrain, out decoration, putOnWall: false);

            var res = new FloorActuator(builder.GetFloorPosition(i.TilePosition, CurrentTile), CurrentTile, targetTile,
                new TypeConstrain(typeof(Creature)), i.GetActionStateX());

            res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
            return res;
        }

        private Actuator Actuator4(ActuatorItemData i)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            builder.PrepareActuatorData(i, out targetTile, out constrain, out decoration, putOnWall: false);

            var res = new FloorActuator(builder.GetFloorPosition(i.TilePosition, CurrentTile), CurrentTile, targetTile,
                new TypeConstrain(typeof(GrabableItem)), i.GetActionStateX());

            res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
            return res;
        }

        private Actuator Actuator1(ActuatorItemData i)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            builder.PrepareActuatorData(i, out targetTile, out constrain, out decoration, putOnWall: false);

            var res = new FloorActuator(
                builder.GetFloorPosition(i.TilePosition, CurrentTile),
                CurrentTile, targetTile,
                new OrConstrain(new IConstrain[]
                {
                    new PartyConstrain(),
                    new TypeConstrain(typeof(Creature)),
                    new TypeConstrain(typeof(GrabableItem))
                }),
                i.GetActionStateX());

            //TODO 14 28 actuator sends Clear message to pit(which open the pit => should be close) 
            res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
            return res;
        }

        private Actuator Actuator2(ActuatorItemData i)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            builder.PrepareActuatorData(i, out targetTile, out constrain, out decoration, putOnWall: false);

            var res = new FloorActuator(builder.GetFloorPosition(i.TilePosition, CurrentTile),
                CurrentTile, targetTile,
                new OrConstrain(new IConstrain[]
                {
                    new TypeConstrain(typeof(Theron)),
                    new PartyConstrain(),
                    new TypeConstrain(typeof(Creature)),
                }), i.GetActionStateX());

            res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
            return res;
        }

        private Actuator PartyActuator3(ActuatorItemData i)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            builder.PrepareActuatorData(i, out targetTile, out constrain, out decoration, putOnWall: false);

            if (i.Data == 0)
                constrain = new PartyConstrain();
            else
                constrain = new PartDirectionConstrain((MapDirection)(i.Data - 1));

            var res = new FloorActuator(builder.GetFloorPosition(i.TilePosition, CurrentTile),
                CurrentTile, targetTile, constrain, i.GetActionStateX());
            
            res.Graphics = new CubeGraphic { Position = res.Position, DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.2f), Texture = decoration };
            return res;
        }

        private Actuator FloorActuatorType8(ActuatorItemData i)
        {  
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            builder.PrepareActuatorData(i, out targetTile, out constrain, out decoration, putOnWall: false);

            return new PartyPossesionActuator(builder.GetFloorPosition(i.TilePosition, CurrentTile), CurrentTile,
                targetTile , constrain, i.GetActionStateX());
        }
    }
}
