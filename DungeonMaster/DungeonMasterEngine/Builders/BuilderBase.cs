using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    public abstract class BuilderBase : IDungonBuilder
    {
        public abstract DungeonLevel GetLevel(int i, Dungeon dungeon, Point? startTile);

        protected void SetupNeighbours(IDictionary<Point, Tile> tilesPositions,  IEnumerable<TileInitializer> initializers)
        {
            foreach (var t in initializers )
            {
                Tile north = null;
                Tile east = null;
                Tile south = null;
                Tile west = null;

                tilesPositions.TryGetValue(t.GridPosition + new Point(0, -1), out north);
                tilesPositions.TryGetValue(t.GridPosition + new Point(1, 0), out east);
                tilesPositions.TryGetValue(t.GridPosition + new Point(0, 1), out south);
                tilesPositions.TryGetValue(t.GridPosition + new Point(-1, 0), out west);
                var neighbours = new TileNeighbours(north, south, east, west);
                t.Neighbours = neighbours;
            }
        }
    }
}
