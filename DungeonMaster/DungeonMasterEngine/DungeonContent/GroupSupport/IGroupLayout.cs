using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface IGroupLayout
    {
        IEnumerable<ISpace> AllSpaces { get; }

        IEnumerable<ISpaceRouteElement> GetToSide(ILayoutable entity, Tile currentTile, MapDirection mapDirection);

        IEnumerable<ISpaceRouteElement> GetToNeighbour(ILayoutable entity, Tile currentTile,  Tile targetTile);
    }
}