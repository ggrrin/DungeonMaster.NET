using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class BasicDecorationFactory : ActuatorFactoryBase
    {
        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {new ActuatorState
        {
            ActuatorType = 0,
            IsLocal = false
        }};

        public BasicDecorationFactory() { }

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            return new DecorationItem(context.GetWallPosition(matchedSequence[0].TilePosition, context.WallActuatorCreator.CurrentTile), context.WallTextures[matchedSequence[0].Decoration - 1]);
        }
    }
}