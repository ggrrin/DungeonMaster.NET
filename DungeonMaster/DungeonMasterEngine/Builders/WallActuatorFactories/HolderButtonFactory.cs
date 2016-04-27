using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class HolderButtonFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = true;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {
            new ActuatorState
            {
                ActuatorType = 1,
                IsLocal = false
            },
        };

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            return new HolderButtonActuator(
                context.GetWallPosition(matchedSequence[0].TilePosition, currentTile),
                context.GetTargetTile(matchedSequence[0]),
                context.WallActuatorCreator.CurrentGrabableItems.Select(x => context.ItemCreator.CreateItem(x, currentTile)),
                matchedSequence[0].GetActionStateX())
            {
                Texture = context.WallTextures[matchedSequence[0].Decoration - 1],
            };
        }
    }
}