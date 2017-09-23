using System;
using DungeonMasterParser.Enums;

namespace DungeonMasterParser.Support
{
    public struct MapPosition
    {
        //        Bits 15-12: Unused
        //        Bits 11-10: Direction
        //            '00' North
        //            '01' East
        //            '10' South
        //            '11' West
        //        Bits 9-5: Y coordinate
        //        Bits 4-0: X coordinate

        public Direction Direction { get; set; }

        public Position Position { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is MapPosition))
                return false;

            var second = (MapPosition)obj;

            return Direction == second.Direction && Position.X == second.Position.X && Position.Y == second.Position.Y;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Position.X, Position.Y, Direction).GetHashCode();
        }

        public override string ToString()
        {
            return $"{Direction} {Position}";
        }
    }
}