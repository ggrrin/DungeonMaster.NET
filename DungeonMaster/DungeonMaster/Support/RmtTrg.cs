namespace DungeonMasterParser.Support
{
    public class RmtTrg :Target
    {
        //    For remote target:
        //        Bits 15-11: Y coordinate of target tile
        //        Bits 10-6: X coordinate of target tile
        //        Bits 5-4: Direction (for a wall tile, determines which wall's face is triggered, North East South West)
        //        Bits 3-0: Unused

        public MapPosition Position { get; set; }


        public override string ToString()
        {
            return Position.ToString();
        }

        public int Direction => (int)Position.Direction;
    }
}