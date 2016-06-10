using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface IGroupLayout
    {
        IEnumerable<ISpace> AllSpaces { get; }

        IEnumerable<ISpaceRouteElement> GetToSide(ILiveEntity liveEntity, Tile currentTile, MapDirection mapDirection);

        IEnumerable<ISpaceRouteElement> GetToNeighbour(ILiveEntity liveEntity, Tile currentTile,  Tile targetTile);
    }
}