using System.Collections.Generic;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class OnethSpace : ISpace
    {
        public INeighbours<ISpace> Neighbours => N4.Empty;
        public Point GridPosition => new Point(0);
        public int LevelIndex => 0;
        public IEnumerable<MapDirection> Sides => MapDirection.Sides;
        public Rectangle Area => new Rectangle(0, 0, 1000, 1000);
        public static OnethSpace Instance { get; } = new OnethSpace();

        private OnethSpace()
        {
            
        }
    }
}