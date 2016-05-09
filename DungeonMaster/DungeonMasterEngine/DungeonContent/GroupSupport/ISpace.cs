using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public interface ISpace : INeighbourable<ISpace>
    {
        IEnumerable<MapDirection> Sides { get; }
        Rectangle Area { get; }
    }
}