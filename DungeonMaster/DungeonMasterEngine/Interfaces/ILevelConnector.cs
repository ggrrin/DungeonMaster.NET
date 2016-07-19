using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Interfaces
{
    public interface ILevelConnector  : ITile
    {
        ITile NextLevelEnter { get; set; }
        int NextLevelIndex { get; }
        Point TargetTilePosition { get; }
    }
}