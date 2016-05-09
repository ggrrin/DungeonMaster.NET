using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class FourthSpace : ISpace
    {
        public INeighbours<ISpace> Neighbours { get; set; }
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