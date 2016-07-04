using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport
{
    public class Medium2GroupLayout : IGroupLayout
    {
        public IEnumerable<ISpace> AllSpaces { get; }
        public static IGroupLayout Instance { get; set; }

        public IEnumerable<ISpaceRouteElement> GetToSide(ISpaceRouteElement currentTile, MapDirection mapDirection, bool useFullSpaces)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISpaceRouteElement> GetToNeighbour(ISpaceRouteElement location, ITile targetTile, bool useFullSpaces)
        {
            throw new NotImplementedException();
        }

        public ISpaceRouteElement GetSpaceElement(ISpace space, ITile tile)
        {
            throw new NotImplementedException();
        }
    }
}