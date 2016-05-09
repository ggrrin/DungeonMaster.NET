using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent
{
    public struct MapDirection
    {
        public static MapDirection North { get; } = new MapDirection(0, -1);
        public static MapDirection East { get; } = new MapDirection(1, 0);
        public static MapDirection South { get; } = new MapDirection(0, 1);
        public static MapDirection West { get; } = new MapDirection(-1, 0);

        public static MapDirection[] AllSides { get; } = { North, East, South, West };

        public static Point operator +(MapDirection s, Point p) => s.RelativeShift + p;
        public static Point operator +(Point p, MapDirection s) => s.RelativeShift + p;

        public static bool IsPureDirection(Point relativeDirection) => relativeDirection == new Point(0, 1) ||
                                                                       relativeDirection == new Point(0, -1) ||
                                                                       relativeDirection == new Point(1, 0) ||
                                                                       relativeDirection == new Point(-1, 0);

        public Point RelativeShift { get; }

        public MapDirection Opposite => new MapDirection(new Point(-1) * RelativeShift);
        public MapDirection NextClockWise => AllSides[(Array.IndexOf(AllSides, this) + 1) % AllSides.Length];
        public MapDirection NextCounterClockWise => NextClockWise.Opposite;

        public MapDirection(Point relativeRelativeShift)
        {
            if (!IsPureDirection(relativeRelativeShift))
                throw new ArgumentException("Invalid Value");
            RelativeShift = relativeRelativeShift;
        }

        public MapDirection(int i, int j) : this(new Point(i, j))
        { }

        public override bool Equals(object obj)
        {
            if (obj is MapDirection)
            {
                return ((MapDirection)obj).RelativeShift.Equals(RelativeShift);
            }
            return false;
        }

        public override int GetHashCode() => RelativeShift.GetHashCode();

        public static bool operator ==(MapDirection l, MapDirection r) => l.RelativeShift == r.RelativeShift;

        public static bool operator !=(MapDirection l, MapDirection r) => !(l == r);

        public static explicit operator MapDirection(int v)
        {
            switch (v)
            {
                case 0:
                    return North;
                case 1:
                    return East;
                case 2:
                    return South;
                case 3:
                    return West;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            if (RelativeShift == new Point(0, 1))
                return "South";
            else if (RelativeShift == new Point(0, -1))
                return "North";
            else if (RelativeShift == new Point(1, 0))
                return "East";
            else if (RelativeShift == new Point(-1, 0))
                return "West";
            else
                return "Undefined.";
        }
    }
}