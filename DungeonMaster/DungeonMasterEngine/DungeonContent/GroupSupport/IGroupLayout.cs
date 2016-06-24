using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface IGroupLayout
    {
        IEnumerable<ISpace> AllSpaces { get; }

        IEnumerable<ISpaceRouteElement> GetToSide(ILiveEntity liveEntity, ITile currentTile, MapDirection mapDirection);

        IEnumerable<ISpaceRouteElement> GetToNeighbour(ILiveEntity liveEntity, ITile currentTile,  ITile targetTile);

        ISpaceRouteElement GetSpaceElement(ISpace space, ITile tile);
    }
}