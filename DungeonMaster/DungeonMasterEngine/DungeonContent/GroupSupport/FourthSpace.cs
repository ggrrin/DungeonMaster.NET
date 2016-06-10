using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
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