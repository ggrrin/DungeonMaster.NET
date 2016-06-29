using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base
{
    public interface ISpaceRouteElement : IStopable 
    {
        ISpace Space { get; }
        ITile Tile { get; }
    }
}