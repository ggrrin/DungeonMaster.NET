using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class LeverSwitchFactory : ActuatorFactoryBase
    {
        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {
            new ActuatorState
            {
                ActuatorType = 1,
                IsLocal = false
            },
            new ActuatorState
            {
                ActuatorType = 1,
                IsLocal = true,
                RotateActuator = true
            }

        };

        public LeverSwitchFactory() { }

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            var leverDown = matchedSequence[1];
            var targetLocation = ((RmtTrg)matchedSequence[0].ActLoc);
            return new LeverActuator(
                context.GetWallPosition(matchedSequence[0].TilePosition, context.WallActuatorCreator.CurrentTile),
                context.GetTargetTile(matchedSequence[0]),
                leverDown.IsRevertable,
                new ActionStateX((ActionState)matchedSequence[0].Action, targetLocation.Direction),
                new ActionStateX((ActionState)matchedSequence[1].Action))
                {
                    UpTexture = context.WallTextures[matchedSequence[0].Decoration - 1],
                    DownTexture = context.WallTextures[leverDown.Decoration - 1]
                };
        }
    }
}