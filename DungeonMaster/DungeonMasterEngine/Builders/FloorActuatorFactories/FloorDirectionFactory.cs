using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Floor;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders.FloorActuatorFactories
{
    public class FloorDirectionFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = null;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] { new ActuatorState { ActuatorType = 3, IsLocal = false } };

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {

            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            context.PrepareActuatorData(matchedSequence[0], out targetTile, out constrain, out decoration, putOnWall: false);

            if (matchedSequence[0].Data == 0)
                constrain = new PartyConstrain();
            else
                constrain = new PartDirectionConstrain((MapDirection)(matchedSequence[0].Data - 1));

            var res = new FloorActuator(context.GetFloorPosition(matchedSequence[0].TilePosition, currentTile), currentTile, constrain, targetTile.ToEnumerable(),
                matchedSequence[0].GetActionStateX().ToEnumerable());

            res.Graphics = new CubeGraphic
            {
                Position = res.Position,
                DrawFaces = CubeFaces.All,
                Outter = true,
                Scale = new Vector3(0.2f),
                Texture = decoration
            };
            return res;
        }
    }
}