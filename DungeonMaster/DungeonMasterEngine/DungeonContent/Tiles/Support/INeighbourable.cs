using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles.Support
{
    public interface INeighbourable<TTile> where TTile : INeighbourable<TTile>
    {
        INeighbors<TTile> Neighbors { get; }
        Point GridPosition { get; }
        int LevelIndex { get; }
    }
}