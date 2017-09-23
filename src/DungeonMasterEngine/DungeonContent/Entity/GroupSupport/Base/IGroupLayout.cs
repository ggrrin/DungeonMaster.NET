using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base
{
    public interface IGroupLayout
    {
        IEnumerable<ISpace> AllSpaces { get; }

        IEnumerable<ISpaceRouteElement> GetToSide(ISpaceRouteElement location, MapDirection mapDirection, bool useFullSpaces);

        IEnumerable<ISpaceRouteElement> GetToNeighbour(ISpaceRouteElement location, ITile targetTile, bool useFullSpaces);

        ISpaceRouteElement GetSpaceElement(ISpace space, ITile tile);
    }
}