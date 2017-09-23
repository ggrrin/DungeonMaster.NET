using System;

namespace DungeonMasterParser.Tiles
{
    public class FloorTileData : TileData
    {
        //Bit 3:
        //    '0' Do not allow random decoration
        //    '1' Allow random decoration
        internal bool AllowRandomDecoration { get; set; }
        public int? RandomDecoration { get; set; }


        //Bit 2 - 0: Unused


        public override T GetTile<T>(ITileCreator<T> t)
        {
            return t.GetTile(this);
        }

        protected override int? GetRandomWallDecoration(DungeonMap map, Random rand)
        {
            int val = rand.Next(29);
            return val < map.FloorDecorationGraphicsCount ? val : (int?)null;
        }

        public override void SetupDecorations(DungeonMap map, Random rand)
        {
            RandomDecoration = AllowRandomDecoration ? GetRandomWallDecoration(map, rand) : null;
        }
    }
}