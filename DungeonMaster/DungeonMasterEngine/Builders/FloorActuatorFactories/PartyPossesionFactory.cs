using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Floor;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser.Items;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders.FloorActuatorFactories
{
    public class PartyPossesionFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = null;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {new ActuatorState
        {
            ActuatorType = 8,
            IsLocal =false 
        }};

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            context.PrepareActuatorData(matchedSequence[0], out targetTile, out constrain, out decoration, putOnWall: false);

            return new PartyPossesionActuator(context.GetFloorPosition(matchedSequence[0].TilePosition, currentTile), currentTile,
                constrain, targetTile.ToEnumerable(), matchedSequence[0].GetActionStateX().ToEnumerable());

        }
    }
}