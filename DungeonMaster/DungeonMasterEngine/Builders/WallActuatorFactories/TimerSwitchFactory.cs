using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class TimerSwitchFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = false;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {
            new ActuatorState
            {
                ActuatorType = 1,
                IsLocal = false
            },
            new ActuatorState
            {
                ActuatorType = 1,
                IsLocal = false,
            }

        };

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            return new SwitchSequenceActuator(
                context.GetWallPosition(matchedSequence[0].TilePosition, context.WallActuatorCreator.CurrentTile),
                matchedSequence.Select(context.GetTargetTile),
                matchedSequence.Select(x => x.GetActionStateX()))
            {
                UpTexture = context.WallTextures[matchedSequence[0].Decoration - 1],
                DownTexture = context.WallTextures[matchedSequence[1].Decoration - 1]
            }; ;
        }
    }
}