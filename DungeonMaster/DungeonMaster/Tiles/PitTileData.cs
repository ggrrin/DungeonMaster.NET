using System;

namespace DungeonMasterParser.Tiles
{
    public class PitTileData : TileData
    {
        //Bit 0:
        //     '0' Normal
        //     '1' Imaginary
        public bool Imaginary { get; set; }

        // Bit 1: Unused
        
        // Bit 2:
        //     '0' Visible
        //     '1' Invisible
        public bool Invisible { get; set; }
        
        // Bit 3:
        //     '0' Closed
        //     '1' Open
        public bool IsOpen { get; set; }

        public override T GetTile<T>(ITileCreator<T> t)
        {
            return t.GetTile(this);
        }

        public override void SetupDecorations(DungeonMap map, Random rand)
        { }
    }
}