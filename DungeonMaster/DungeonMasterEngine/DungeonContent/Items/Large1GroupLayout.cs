using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public class Large1GroupLayout : IGroupLayout
    {
        public IEnumerable<ISpace> AllSpaces { get; }

        public IEnumerable<ISpaceRouteElement> GetToSide(ILayoutable entity, Tile currentTile, MapDirection mapDirection)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISpaceRouteElement> GetToNeighbour(ILayoutable entity, Tile currentTile, Tile targetTile)
        {
            throw new NotImplementedException();
        }
    }
}