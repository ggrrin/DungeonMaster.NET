namespace DungeonMasterParser
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


        public override string ToString()
        {
            return $"{Direction} {Position}";
        }
    }
}