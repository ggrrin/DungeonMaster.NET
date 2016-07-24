using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles.Initializers
{
    public class LogicTileInitializer : TileInitializer
    {
        public new event Initializer<LogicTileInitializer> Initializing;

        public LogicActuator LogicActuator { get; set; }

        public override void SetupNeighbours(IDictionary<Point, Tile> tilesPositions)
        { }

        protected override void OnInitializing()
        {
            base.OnInitializing();
            Initializing?.Invoke(this);
        }
    }
}