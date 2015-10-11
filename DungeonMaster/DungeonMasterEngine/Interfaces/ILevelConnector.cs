using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Interfaces
{
    public interface ILevelConnector 
    {
        Tile NextLevelEnter { get; set; }
        int NextLevelIndex { get; }
        Point TargetTilePosition { get; }
        Point GridPosition { get; }
        
    }
}