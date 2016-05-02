using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterParser.Items;

namespace DungeonMasterEngine.Builders.FloorActuatorFactories
{
    public class FloorDecorationFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = null;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {new ActuatorState {ActuatorType = 0}};

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            return new DecorationItem(context.GetFloorPosition(matchedSequence[0].TilePosition, currentTile), context.FloorTextures[matchedSequence[0].Decoration - 1]);
        }
    }
}
