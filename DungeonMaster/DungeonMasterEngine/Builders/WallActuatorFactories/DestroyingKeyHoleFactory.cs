using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser.Items;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders.WallActuatorFactories
{
    public class DestroyingKeyHoleFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = false;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {new ActuatorState
        {
            ActuatorType = 4,
        }};

        public DestroyingKeyHoleFactory() { }

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            Texture2D decoration;
            IConstrain constrain;
            Tile targetTile;
            context.PrepareActuatorData(matchedSequence[0], out targetTile, out constrain, out decoration, putOnWall: true);
            return new KeyHoleActuator(context.GetWallPosition(matchedSequence[0].TilePosition, context.WallActuatorCreator.CurrentTile),
                Enumerable.Repeat(targetTile,1), Enumerable.Repeat(matchedSequence[0].GetActionStateX(),1), constrain, destroyItem: true)
            {
                DecorationTexture = decoration
            };
        }
    }
}