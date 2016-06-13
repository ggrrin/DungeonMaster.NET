using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class Large1GroupLayout : IGroupLayout
    {
        public IEnumerable<ISpace> AllSpaces { get; }

        public IEnumerable<ISpaceRouteElement> GetToSide(ILiveEntity liveEntity, ITile currentTile, MapDirection mapDirection)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISpaceRouteElement> GetToNeighbour(ILiveEntity liveEntity, ITile currentTile, ITile targetTile)
        {
            throw new NotImplementedException();
        }
    }
}