using DungeonMasterParser.Tiles;

namespace DungeonMasterParser
{
    public class TeleporterTile : Tile
    {
        //Bit 1 - 0: Unused

        //Bit 2: Visibility
        //        '0' Invisible
        //        '1' Visible(blue haze)
        public bool IsVisible { get; set; }

        //    Bit 3:
        //        '0' Closed
        //        '1' Open
        public bool IsOpen { get; set; }

        public override T GetTile<T>(ITileCreator<T> t)
        {
            return t.GetTile(this);
        }

    }
}