using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class BasicAlcoveFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = null; 

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {new ActuatorState
        {
            ActuatorType = 0,
        }};

        public BasicAlcoveFactory() { }

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            var items = context.WallActuatorCreator.CurrentGrabableItems.Select(x => context.ItemCreator.CreateItem(x, currentTile));
            return new AlcoveActuator(context.GetWallPosition(matchedSequence[0].TilePosition, context.WallActuatorCreator.CurrentTile), items)
            {
                AlcoveTexture = context.WallTextures[matchedSequence[0].Decoration - 1]
            };
        }
    }
}