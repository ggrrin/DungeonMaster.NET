using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface ISpace : INeighbourable<ISpace>
    {
        IEnumerable<MapDirection> Sides { get; }
        Rectangle Area { get; }
    }
}