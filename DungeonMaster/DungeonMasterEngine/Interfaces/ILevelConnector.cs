using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Interfaces
{
    public interface ILevelConnector  : ITile
    {
        Tile NextLevelEnter { get; set; }
        int NextLevelIndex { get; }
        Point TargetTilePosition { get; }
    }
}