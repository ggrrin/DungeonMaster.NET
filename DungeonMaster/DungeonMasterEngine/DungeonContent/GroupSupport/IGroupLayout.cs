using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface IGroupLayout
    {
        IEnumerable<ISpace> AllSpaces { get; }

        IEnumerable<ISpaceRouteElement> GetToSide(IEntity entity, Tile currentTile, MapDirection mapDirection);

        IEnumerable<ISpaceRouteElement> GetToNeighbour(IEntity entity, Tile currentTile,  Tile targetTile);
    }
}