using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    public abstract class BuilderBase : IDungonBuilder
    {
        public abstract DungeonLevel GetLevel(int i, Point? startTile);

        protected void SetupNeighbours(IDictionary<Point, Tile> tilesPositions,  IEnumerable<TileInitializer> initializers)
        {
            foreach (var t in initializers )
            {
                t.SetupNeighbours(tilesPositions);
            }
        }


    }
}
