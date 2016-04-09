using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent
{
    class MultiNeighbours : Neighbours
    {
        public Neighbours FirstNeighbour { get; }
        public Neighbours SecondNeighbour { get; }

        public MultiNeighbours(Neighbours firstNeighbour, Neighbours secondNeighbour)
        {
            FirstNeighbour = firstNeighbour;
            SecondNeighbour = secondNeighbour;
        }


        public override Tile East
        {
            get
            {
                if (FirstNeighbour.East == null)
                    return SecondNeighbour.East;
                else
                    return FirstNeighbour.East;
            }
        }

        public override Tile North
        {
            get
            {
                if (FirstNeighbour.North == null)
                    return SecondNeighbour.North;
                else
                    return FirstNeighbour.North;
            }
        }

        public override Tile South
        {
            get
            {
                if (FirstNeighbour.South == null)
                    return SecondNeighbour.South;
                else
                    return FirstNeighbour.South;
            }
        }

        public override Tile West
        {
            get
            {
                if (FirstNeighbour.West == null)
                    return SecondNeighbour.West;
                else
                    return FirstNeighbour.West;
            }
        }

    }
}
