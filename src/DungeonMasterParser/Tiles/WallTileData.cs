using System;

namespace DungeonMasterParser.Tiles
{
    public class WallTileData :  TileData
    {

//        Note: The number of graphics available for random decorations must be smaller than the number of graphics available in the map.The first graphics in the lists of available graphics (defined at the end of each map data) are used for random decorations.
//Random decorations are only placed on the sides of 'Wall' and 'Floor' tiles if they allow them (four bits to allow each side of the tile to receive a decoration). The program generates a random number between 0 and 29. If the random number is lesser than the number of graphics available for random decorations, then the decoration is displayed.

        //Bit 3:
        //    '0' Do not allow random decoration on North side
        //    '1' Allow random decoration on North side
        internal bool AllowNorthRandomDecoration { get; set; }
        public int? NorthRandomDecoration { get; set; }

        //Bit 2:
        //    '0' Do not allow random decoration on East side
        //    '1' Allow random decoration on East side
        internal bool AllowEastRandomDecoration { get; set; }
        public int? EastRandomDecoration { get; set; }

        //Bit 1:
        //    '0' Do not allow random decoration on South side
        //    '1' Allow random decoration on South side
        internal bool AllowSouthRandomDecoration { get; set; }
        public int? SouthRandomDecoration { get; set; }

        //Bit 0:
        //    '0' Do not allow random decoration on West side
        //    '1' Allow random decoration on West side
        internal bool AllowWestRandomDecoration { get; set; }
        public int? WestRandomDecoration { get; set; }

        

        public override void SetupDecorations(DungeonMap map, Random rand)
        {
            NorthRandomDecoration = AllowNorthRandomDecoration ? GetRandomWallDecoration(map, rand) : null;
            EastRandomDecoration = AllowEastRandomDecoration ? GetRandomWallDecoration(map, rand) : null;
            SouthRandomDecoration = AllowSouthRandomDecoration ? GetRandomWallDecoration(map, rand) : null;
            WestRandomDecoration = AllowWestRandomDecoration ? GetRandomWallDecoration(map, rand) : null;
        }


        public override T GetTile<T>(ITileCreator<T> t)
        {
            return t.GetTile(this);
        }
    }
}