using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent
{
    class MultiTileNeighbours : TileNeighbours
    {
        public TileNeighbours FirstTileNeighbour { get; }
        public TileNeighbours SecondTileNeighbour { get; }

        public MultiTileNeighbours(TileNeighbours firstTileNeighbour, TileNeighbours secondTileNeighbour)
        {
            FirstTileNeighbour = firstTileNeighbour;
            SecondTileNeighbour = secondTileNeighbour;
        }


        public override ITile East
        {
            get
            {
                if (FirstTileNeighbour.East == null)
                    return SecondTileNeighbour.East;
                else
                    return FirstTileNeighbour.East;
            }
        }

        public override ITile North
        {
            get
            {
                if (FirstTileNeighbour.North == null)
                    return SecondTileNeighbour.North;
                else
                    return FirstTileNeighbour.North;
            }
        }

        public override ITile South
        {
            get
            {
                if (FirstTileNeighbour.South == null)
                    return SecondTileNeighbour.South;
                else
                    return FirstTileNeighbour.South;
            }
        }

        public override ITile West
        {
            get
            {
                if (FirstTileNeighbour.West == null)
                    return SecondTileNeighbour.West;
                else
                    return FirstTileNeighbour.West;
            }
        }

    }
}
