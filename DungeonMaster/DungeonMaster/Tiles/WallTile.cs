using System;
using DungeonMasterParser.Tiles;

namespace DungeonMasterParser
{
    public class WallTile :  Tile
    {
        //Bit 3:
        //    '0' Do not allow random decoration on North side
        //    '1' Allow random decoration on North side
        public bool AllowNorthRandomDecoration { get; set; }

        //Bit 2:
        //    '0' Do not allow random decoration on East side
        //    '1' Allow random decoration on East side
        public bool AllowEastRandomDecoration { get; set; }

        //Bit 1:
        //    '0' Do not allow random decoration on South side
        //    '1' Allow random decoration on South side
        public bool AllowSouthRandomDecoration { get; set; }

        //Bit 0:
        //    '0' Do not allow random decoration on West side
        //    '1' Allow random decoration on West side
        public bool AllowWestRandomDecoration { get; set; }

        public override T GetTile<T>(ITileCreator<T> t)
        {
            return t.GetTile(this);
        }
    }
}