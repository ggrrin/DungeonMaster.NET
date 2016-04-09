using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent
{
    public class DungeonLevel
    {        
        private Dungeon dungeon;

        public Tile StartTile { get; }

        public int LevelIndex { get; }        

        public Texture2D MiniMap { get; }

        public IEnumerable<Tile> Tiles { get; }

        public IReadOnlyDictionary<Point, Tile> TilesPositions { get; }

        public DungeonLevel(Dungeon d, IEnumerable<Tile> t, int levelIndex, IReadOnlyDictionary<Point,Tile> positions, Tile startTile, Texture2D minimap)
        {
            TilesPositions = positions;

            dungeon = d;
            Tiles = t;
            StartTile = startTile;
            LevelIndex = levelIndex;
            MiniMap = minimap;
        }
    }
}