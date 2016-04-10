using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    public abstract class BuilderBase : IDungonBuilder
    {
        public abstract DungeonLevel GetLevel(int i, Dungeon dungeon, Point? startTile);

        protected void SetupNeighbours(IDictionary<Point, Tile> tilesPositions, IList<Tile> tiles)
        {
            foreach (var t in tiles)
            {
                Tile north = null;
                Tile east = null;
                Tile south = null;
                Tile west = null;

                var neighbours = new Neighbours();
                if (tilesPositions.TryGetValue(t.GridPosition + new Point(0, -1), out north))
                    neighbours.North = north;

                if (tilesPositions.TryGetValue(t.GridPosition + new Point(1, 0), out east))
                    neighbours.East = east;

                if (tilesPositions.TryGetValue(t.GridPosition + new Point(0, 1), out south))
                    neighbours.South = south;

                if (tilesPositions.TryGetValue(t.GridPosition + new Point(-1, 0), out west))
                    neighbours.West = west;
                t.Neighbours = neighbours;
            }
        }
    }
}
