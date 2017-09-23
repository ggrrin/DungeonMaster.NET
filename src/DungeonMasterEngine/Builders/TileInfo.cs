using DungeonMasterParser.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    public struct TileInfo<T> where T : TileData
    {
        public T Tile { get; set; }

        public Point Position { get; set; }
    }
}