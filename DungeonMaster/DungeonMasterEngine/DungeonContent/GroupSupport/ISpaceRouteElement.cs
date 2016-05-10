using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public interface ISpaceRouteElement : IStopable 
    {
        ISpace Space { get; }
        Tile Tile { get; }
    }
}