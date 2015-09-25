using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonMasterEngine
{
    public class DungeonLevel
    {        
        private Dungeon dungeon;
        private IReadOnlyList<Tile> tiles;
        public Tile StartTile { get; }

        public int LevelIndex { get; }        

        public IReadOnlyDictionary<Point, Tile> TilesPositions { get; }

        public Texture2D MiniMap { get; }

        public DungeonLevel(Dungeon d, IReadOnlyList<Tile> t, int levelIndex, IReadOnlyDictionary<Point,Tile> positions, Tile startTile, Texture2D minimap)
        {
            TilesPositions = positions;

            dungeon = d;
            tiles = t;
            StartTile = startTile;
            LevelIndex = levelIndex;
            MiniMap = minimap;
        }
    }
}