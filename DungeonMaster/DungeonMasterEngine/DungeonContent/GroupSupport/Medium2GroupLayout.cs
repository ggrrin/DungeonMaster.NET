using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class Medium2GroupLayout : IGroupLayout
    {
        public IEnumerable<ISpace> AllSpaces { get; }
        public static IGroupLayout Instance { get; set; }

        public IEnumerable<ISpaceRouteElement> GetToSide(ILiveEntity liveEntity, ITile currentTile, MapDirection mapDirection)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISpaceRouteElement> GetToNeighbour(ILiveEntity liveEntity, ITile currentTile, ITile targetTile)
        {
            throw new NotImplementedException();
        }

        public ISpaceRouteElement GetSpaceElement(ISpace space, ITile tile)
        {
            throw new NotImplementedException();
        }
    }
}