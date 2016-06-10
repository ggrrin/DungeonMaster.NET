using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class Medium2GroupLayout : IGroupLayout
    {
        public IEnumerable<ISpace> AllSpaces { get; }

        public IEnumerable<ISpaceRouteElement> GetToSide(ILiveEntity liveEntity, Tile currentTile, MapDirection mapDirection)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISpaceRouteElement> GetToNeighbour(ILiveEntity liveEntity, Tile currentTile, Tile targetTile)
        {
            throw new NotImplementedException();
        }
    }
}