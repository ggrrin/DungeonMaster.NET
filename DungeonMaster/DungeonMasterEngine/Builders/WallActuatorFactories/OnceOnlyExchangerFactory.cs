using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class OnceOnlyExchangerFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = null;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {new ActuatorState {ActuatorType = 13, IsLocal = true, RotateActuator = false}};


        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            var data = matchedSequence[0];
            var constrain = new GrabableItemConstrain(context.GetItemFactory(data.Data), invertConstraion: false);
            var item = context.WallActuatorCreator.CurrentGrabableItems.Select(k => new LegacyItemCreator(context).CreateItem(k, currentTile)).SingleOrDefault();
            return new ExchangerActuator(context.GetWallPosition(data.TilePosition, context.WallActuatorCreator.CurrentTile), item, constrain, onceOnly:true)
            {
                DecorationActivated = context.WallTextures[data.Decoration - 1],
                DecorationDeactived = context.WallTextures[data.Decoration - 1],
            };
        }
    }
}