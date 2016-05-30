using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Floor;
using DungeonMasterEngine.DungeonContent.Constrains;
using DungeonMasterEngine.DungeonContent.EntitySupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using DungeonMasterParser.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders.FloorActuatorFactories
{
    public class TPCFactory : ActuatorFactoryBase
    {
        public override bool? RequireItem { get; } = null;

        public override IReadOnlyList<ActuatorState> MatchingSequence { get; } = new[] {
            new ActuatorState
            {
                ActuatorType = 2,
                IsLocal =false 
            } };

        public override Actuator CreateItem(LegacyMapBuilder context, Tile currentTile, IReadOnlyList<ActuatorItemData> matchedSequence)
        {
            IConstrain constrain = null;
            Tile targetTile = null;
            Texture2D decoration = null;
            context.PrepareActuatorData(matchedSequence[0], out targetTile, out constrain, out decoration, putOnWall: false);

            var res = new FloorActuator(context.GetFloorPosition(matchedSequence[0].TilePosition, currentTile), currentTile,
                new OrConstrain(new IConstrain[] { new TypeConstrain(typeof(Theron)), new PartyConstrain(), new TypeConstrain(typeof(Creature)), }),
                targetTile.ToEnumerable(), matchedSequence[0].GetActionStateX().ToEnumerable());

            res.Graphics = new CubeGraphic
            {
                Position = res.Position,
                DrawFaces = CubeFaces.All,
                Outter = true,
                Scale = new Vector3(0.2f),
                Texture = decoration
            };
            return res;
        }
    }
}