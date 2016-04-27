using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class ButtonFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = false;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {
            new ActuatorState
            {
                ActuatorType = 1,
                IsLocal = false
            },
        };

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            return new Button(
                context.GetWallPosition(matchedSequence[0].TilePosition, currentTile),
                context.GetTargetTile(matchedSequence[0]),
                matchedSequence[0].GetActionStateX())
            {
                Texture = context.WallTextures[matchedSequence[0].Decoration - 1],
            };
        }
    }
}