using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent
{
    class MultiTileNeighbors : TileNeighbors
    {
        public TileNeighbors FirstTileNeighbor { get; }
        public TileNeighbors SecondTileNeighbor { get; }

        public MultiTileNeighbors(TileNeighbors firstTileNeighbor, TileNeighbors secondTileNeighbor)
        {
            FirstTileNeighbor = firstTileNeighbor;
            SecondTileNeighbor = secondTileNeighbor;
        }


        public override ITile East
        {
            get
            {
                if (FirstTileNeighbor.East == null)
                    return SecondTileNeighbor.East;
                else
                    return FirstTileNeighbor.East;
            }
        }

        public override ITile North
        {
            get
            {
                if (FirstTileNeighbor.North == null)
                    return SecondTileNeighbor.North;
                else
                    return FirstTileNeighbor.North;
            }
        }

        public override ITile South
        {
            get
            {
                if (FirstTileNeighbor.South == null)
                    return SecondTileNeighbor.South;
                else
                    return FirstTileNeighbor.South;
            }
        }

        public override ITile West
        {
            get
            {
                if (FirstTileNeighbor.West == null)
                    return SecondTileNeighbor.West;
                else
                    return FirstTileNeighbor.West;
            }
        }

    }
}
