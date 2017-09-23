using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport
{
    public class FullTileLayout : IGroupLayout
    {
        public IEnumerable<ISpace> AllSpaces => OnethSpace.Instance.ToEnumerable();

        public IEnumerable<ISpaceRouteElement> GetToSide(ISpaceRouteElement location, MapDirection mapDirection, bool useFullSpaces)
        {
            return AllSpaces.Select(x => new OnethSpaceRouteElement(x, location.Tile));
        }

        public IEnumerable<ISpaceRouteElement> GetToNeighbour(ISpaceRouteElement location, ITile targetTile, bool useFullSpaces)
        {
            return AllSpaces.Select(x => new OnethSpaceRouteElement(x, location.Tile));
        }

        public ISpaceRouteElement GetSpaceElement(ISpace space, ITile tile)
        {
            return new OnethSpaceRouteElement(space, tile);
        }

        public static FullTileLayout Instance { get; } = new FullTileLayout();

        private FullTileLayout()
        {
            
        }
    }
}