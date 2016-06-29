using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles.Support
{
    public abstract class TileInitializer : InitializerBase
    {
        public event Initializer<TileInitializer> Initializing;
        public event Initializer<TileInitializer> Initialized;

        public DungeonLevel Level { get; set; }
        public Point GridPosition { get; set; }
        public TileNeighbors Neighbors { get; set; }
        public IEnumerable<ILiveEntity> Creatures { get; set; }

        public virtual void SetupNeighbours(IDictionary<Point, Tile> tilesPositions)
        {
            Tile north = null;
            Tile east = null;
            Tile south = null;
            Tile west = null;

            tilesPositions.TryGetValue(GridPosition + new Point(0, -1), out north);
            tilesPositions.TryGetValue(GridPosition + new Point(1, 0), out east);
            tilesPositions.TryGetValue(GridPosition + new Point(0, 1), out south);
            tilesPositions.TryGetValue(GridPosition + new Point(-1, 0), out west);
            var neighbours = new TileNeighbors(Check(north) , Check(south), Check(east), Check(west));
            Neighbors = neighbours;
        }

        private Tile Check(Tile t)
        {
            if (t is LogicTile)
                return null;
            else
                return t;
        }


        protected override void OnInitialing()
        {
            Initializing?.Invoke(this);
        }

        protected override void OnInitialized()
        {
            Initialized?.Invoke(this);
        }
    }
}