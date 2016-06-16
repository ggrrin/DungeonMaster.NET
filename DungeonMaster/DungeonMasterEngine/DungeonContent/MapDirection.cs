using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent
{
    public struct MapDirection
    {
        public static MapDirection North { get; } = new MapDirection(0, -1);
        public static MapDirection East { get; } = new MapDirection(1, 0);
        public static MapDirection South { get; } = new MapDirection(0, 1);
        public static MapDirection West { get; } = new MapDirection(-1, 0);
        public static MapDirection Down { get; } = new MapDirection(-1);
        public static MapDirection Up { get; } = new MapDirection(1);

        private static readonly MapDirection[] sides = { North, East, South, West };
        public static IReadOnlyList<MapDirection> Sides { get; } = sides;

        private static readonly  MapDirection[] horizontalDirections = { Down, Up };
        public static IReadOnlyList<MapDirection> HorizontalDirections => horizontalDirections;

        public static IEnumerable<MapDirection> AllDirections => Sides.Concat(HorizontalDirections);

        public static Point operator +(MapDirection s, Point p) => s.RelativeShift + p;
        public static Point operator +(Point p, MapDirection s) => s.RelativeShift + p;

        public static bool IsPureDirection(Point relativeDirection) => relativeDirection == new Point(0, 1) ||
                                                                       relativeDirection == new Point(0, -1) ||
                                                                       relativeDirection == new Point(1, 0) ||
                                                                       relativeDirection == new Point(-1, 0);

        public Point RelativeShift { get; }
        public int HorizontalShift { get; }

        public MapDirection Opposite
        {
            get
            {
                if (HorizontalShift == 0)
                    return new MapDirection(new Point(-1) * RelativeShift);
                else
                    return new MapDirection(-HorizontalShift);
            }
        }

        public MapDirection NextClockWise
        {
            get
            {
                if (HorizontalShift == 0)
                    return Sides[(Array.IndexOf(sides, this) + 1) % sides.Length];
                else
                    return new MapDirection(-HorizontalShift);
            }
        }


        public int Index
        {
            get
            {
                if (HorizontalShift == 0)
                    return Array.IndexOf(sides, this);
                else
                    return 4 + Array.IndexOf(horizontalDirections, this);
            }
        }

        public MapDirection NextCounterClockWise => NextClockWise.Opposite;

        public MapDirection(Point relativeRelativeShift) : this()
        {
            if (!IsPureDirection(relativeRelativeShift))
                throw new ArgumentException("Invalid Value");
            RelativeShift = relativeRelativeShift;
        }

        public MapDirection(int horizontalShift) : this()
        {
            if (!IsHroizontalOK(horizontalShift))
                throw new ArgumentException("Invalid Value");
            HorizontalShift = horizontalShift;
        }

        public DirectionShift ShiftType => HorizontalShift == 0 ? DirectionShift.VerticalShift : DirectionShift.HorizontalShift;

        public static bool IsHroizontalOK(int horizontalShift)
        {
            switch (horizontalShift)
            {
                case 1:
                case -1:
                    return true;
                default:
                    return false;
            }
        }

        public MapDirection GetRotated(int count)
        {
            bool clockwise = count >= 0;
            if (!clockwise)
                count *= -1;

            MapDirection res = this;
            for (int i = 0; i < count; i++)
                res = clockwise ? res.NextClockWise : res.NextCounterClockWise;

            return res;

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

        public static bool operator ==(MapDirection l, MapDirection r) => l.HorizontalShift == r.HorizontalShift && l.RelativeShift == r.RelativeShift;

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

    public enum DirectionShift
    {
        HorizontalShift, VerticalShift
        
    }
}