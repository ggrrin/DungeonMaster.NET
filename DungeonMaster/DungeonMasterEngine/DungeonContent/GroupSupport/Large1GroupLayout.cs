using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class Large1GroupLayout : IGroupLayout
    {
        public IEnumerable<ISpace> AllSpaces { get; }

        public IEnumerable<ISpaceRouteElement> GetToSide(IEntity entity, Tile currentTile, MapDirection mapDirection)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISpaceRouteElement> GetToNeighbour(IEntity entity, Tile currentTile, Tile targetTile)
        {
            throw new NotImplementedException();
        }
    }
}