using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Floor;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders.FloorActuatorFactories
{
    public class MultiFloorDirectionFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = null;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[]
        {
            new ActuatorState { ActuatorType = 3, IsLocal = false },
            new ActuatorState { ActuatorType = 3, IsLocal = false },
        };

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {

            IConstrain constrain = null;
            if (matchedSequence[0].Data == 0)
                constrain = new PartyConstrain();
            else
                constrain = new PartDirectionConstrain((MapDirection)(matchedSequence[0].Data - 1));

            var res = new FloorActuator(context.GetFloorPosition(matchedSequence[0].TilePosition, currentTile), currentTile, constrain,
                matchedSequence.Select(context.GetTargetTile), matchedSequence.Select(x => x.GetActionStateX()));

            res.Graphics = new CubeGraphic
            {
                Position = res.Position,
                DrawFaces = CubeFaces.All,
                Outter = true,
                Scale = new Vector3(0.2f),
                Texture = context.GetTexture(matchedSequence.First(), putOnWall:false) 
            };
            return res;
        }
    }
}