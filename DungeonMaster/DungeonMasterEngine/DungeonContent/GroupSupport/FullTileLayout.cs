using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class FullTileLayout : IGroupLayout
    {
        public IEnumerable<ISpace> AllSpaces => OnethSpace.Instance.ToEnumerable();

        public IEnumerable<ISpaceRouteElement> GetToSide(ILiveEntity liveEntity, ITile currentTile, MapDirection mapDirection)
        {
            return AllSpaces.Select(x => new OnethSpaceRouteElement(x, currentTile));
        }

        public IEnumerable<ISpaceRouteElement> GetToNeighbour(ILiveEntity liveEntity, ITile currentTile, ITile targetTile)
        {
            return AllSpaces.Select(x => new OnethSpaceRouteElement(x, currentTile));
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