using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class BasicExchangerFactoryReverse : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = null;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] { new ActuatorState
        {
            ActuatorType = 0,
            IsLocal = true,
            RotateActuator = false
        },
            new ActuatorState
            {
                ActuatorType = 13,
                IsLocal = true,
                RotateActuator = true
            }
        };

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            var data = matchedSequence[1];
            var constrain = new GrabableItemConstrain(data.Data, invertConstraion: false);
            var item = context.WallActuatorCreator.CurrentGrabableItems.Select(k => new LegacyItemCreator(context).CreateItem(k, currentTile)).SingleOrDefault();
            return new ExchangerActuator(context.GetWallPosition(data.TilePosition, context.WallActuatorCreator.CurrentTile), item, constrain)
            {
                DecorationActivated = context.WallTextures[data.Decoration - 1],
                DecorationDeactived = context.WallTextures[matchedSequence[0].Decoration - 1]// bug
            };
        }
    }
}