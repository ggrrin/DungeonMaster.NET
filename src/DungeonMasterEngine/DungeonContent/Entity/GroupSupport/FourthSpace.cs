using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport
{
    public class FourthSpace : ISpace
    {
        public INeighbors<ISpace> Neighbors { get; set; }
        public Point GridPosition { get; }
        public int LevelIndex => 0;
        public IEnumerable<MapDirection> Sides { get; }
        public Rectangle Area => new Rectangle(new Point(500) * GridPosition, new Point(500));

        public FourthSpace(int x, int y, IEnumerable<MapDirection> side) : this(new Point(x, y), side)
        { }

        public FourthSpace(Point gridPosition, IEnumerable<MapDirection> side)
        {
            GridPosition = gridPosition;
            Sides = side;
        }
    }
}