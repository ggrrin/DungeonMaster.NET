namespace DungeonMasterParser
{
    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Position operator+(Position l, Position r)
        {
            return new Position { X = l.X + r.X, Y = l.Y + r.Y };
        }

        public override string ToString()
        {
            return $"X = {X} ; Y = {Y}";
        }
    }
}