using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public interface INeighbourable<TTile> where TTile : INeighbourable<TTile>
    {
        INeighbours<TTile> Neighbours { get; }
        Point GridPosition { get; }
        int LevelIndex { get; }
    }
}