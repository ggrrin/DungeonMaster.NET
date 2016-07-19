using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent
{
    public class DungeonLevel
    {        
        public Tile StartTile { get; }

        public int LevelIndex { get; }        

        public Texture2D MiniMap { get; }

        public IEnumerable<Tile> Tiles { get; }

        public IReadOnlyDictionary<Point, Tile> TilesPositions { get; }
        public int Difficulty { get; }
        public ICollection<IUpdate> Updateables { get; } = new HashSet<IUpdate>();

        public DungeonLevel(int levelIndex, IReadOnlyDictionary<Point,Tile> positions, Tile startTile, Texture2D minimap, int difficulty)
        {
            TilesPositions = positions;

            Tiles = positions.Values;
            StartTile = startTile;
            LevelIndex = levelIndex;
            MiniMap = minimap;
            Difficulty = difficulty;
        }

        public void Update(GameTime time)
        {
            foreach (var updateable in Updateables.ToArray())
                updateable.Update(time);
        }
    }
}