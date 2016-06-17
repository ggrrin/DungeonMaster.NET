using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class LogicTileInitializer : TileInitializer
    {
        public new event Initializer<LogicTileInitializer> Initializing;

        public LogicActuator LogicActuator { get; set; }

        public override void SetupNeighbours(IDictionary<Point, Tile> tilesPositions)
        { }

        protected override void OnInitialing()
        {
            base.OnInitialing();
            Initializing?.Invoke(this);
        }
    }
}