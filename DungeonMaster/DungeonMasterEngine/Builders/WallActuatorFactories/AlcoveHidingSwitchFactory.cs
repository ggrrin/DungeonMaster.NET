using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class AlcoveHidingSwitchFactory : ActuatorFactoryBase
    {
        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {new ActuatorState
        {
            ActuatorType = 1,
            IsLocal = true
        },
            new ActuatorState
            {
                ActuatorType = 0,
                IsLocal = true
            } };

        public AlcoveHidingSwitchFactory() { }

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            return new AlcoveActuator(context.GetWallPosition(matchedSequence[0].TilePosition, context.WallActuatorCreator.CurrentTile), context.WallActuatorCreator.CurrentGrabableItems.Select(x => context.ItemCreator.CreateItem(x, currentTile)))
            {
                Hidden = true,
                AlcoveTexture = context.WallTextures[matchedSequence[1].Decoration - 1],
                HideoutTexture = context.WallTextures[matchedSequence[0].Decoration - 1]
            };
        }
    }
}