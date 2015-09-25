using DungeonMasterParser.Tiles;

namespace DungeonMasterParser
{
    public class FloorTile : Tile
    {
        //Bit 3:
        //    '0' Do not allow random decoration
        //    '1' Allow random decoration
        public bool AllowRandomDecoration { get; set; }


        //Bit 2 - 0: Unused


        public override T GetTile<T>(ITileCreator<T> t)
        {
            return t.GetTile(this);
        }
    }
}