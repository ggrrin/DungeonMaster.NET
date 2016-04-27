using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class TimerSwitchFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = false;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {
            new ActuatorState
            {
                Action =  ActionType.Set,
                ActuatorType = 1,
                IsLocal = false
            },
            new ActuatorState
            {
                Action =  ActionType.Clear,
                ActuatorType = 1,
                IsLocal = false,
            }

        };

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            var loc1 = (RemoteTarget)matchedSequence[0].ActionLocation;
            var loc2 = (RemoteTarget)matchedSequence[1].ActionLocation;

            if (loc1.Equals(loc2))
            {
                return new TimerSwitchActuator(
                    context.GetWallPosition(matchedSequence[0].TilePosition, context.WallActuatorCreator.CurrentTile),
                    context.GetTargetTile(matchedSequence[0]),
                    matchedSequence[0].GetActionStateX(),
                    matchedSequence[1].GetActionStateX())
                {
                    UpTexture = context.WallTextures[matchedSequence[0].Decoration - 1],
                    DownTexture = context.WallTextures[matchedSequence[1].Decoration - 1]
                };
            }
            else
                throw new InvalidOperationException();
        }
    }
}