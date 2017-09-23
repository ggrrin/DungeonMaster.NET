using System;

namespace DungeonMasterParser.Tiles
{
    public class TrickTileData : TileData
    {
        //Bit 0:
        //    '0' False
        //    '1' Imaginary
        public bool IsImaginary { get; set; }

        //Bit 1: Unused

        //Bit 2:
        //    '0' Closed
        //    '1' Open
        public bool IsOpen { get; set; }

        //Bit 3:
        //    '0' Do not allow random decoration
        //    '1' Allow random decoration
        internal bool AllowRandomDecoration { get; set; }
        public int? RandomDecoration { get; set; }

        public override T GetTile<T>(ITileCreator<T> t)
        {
            return t.GetTile(this);
        }

        public override void SetupDecorations(DungeonMap map, Random rand)
        {
            RandomDecoration = AllowRandomDecoration ? GetRandomWallDecoration(map, rand) : null;
        }
    }
}