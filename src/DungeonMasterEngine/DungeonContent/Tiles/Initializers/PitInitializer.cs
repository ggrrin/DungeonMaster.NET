using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles.Initializers
{
    public class PitInitializer : FloorInitializer
    {
        public new event Initializer<PitInitializer> Initializing;
        public bool Imaginary { get; set; }

        public bool Invisible { get; set; }
        
        public bool IsOpen { get; set; }

        public override void SetupNeighbours(IDictionary<Point, Tile> tilesPositions)
        {
            Tile north = null;
            Tile east = null;
            Tile south = null;
            Tile west = null;

            tilesPositions.TryGetValue(GridPosition + new Point(0, -1), out north);
            tilesPositions.TryGetValue(GridPosition + new Point(1, 0), out east);
            tilesPositions.TryGetValue(GridPosition + new Point(0, 1), out south);
            tilesPositions.TryGetValue(GridPosition + new Point(-1, 0), out west);
            Neighbors = new PitTileNeighbors(north, south, east, west);
        }

        protected override void OnInitializing()
        {
            base.OnInitializing();
            Initializing?.Invoke(this);
        }
    }
}