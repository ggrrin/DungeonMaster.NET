using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class PitTileNeighbours : TileNeighbours
    {
        public ITile Up { get; set; }
        public ITile Down { get; set; }

        public override ITile GetTile(MapDirection mapDirection)
        {
            var res = base.GetTile(mapDirection);
            if (res == null)
            {
                if (mapDirection == MapDirection.Up)
                    return Up;
                else if (mapDirection == MapDirection.Down)
                    return Down;
                else
                    return null;
            }
            else
                return res;
        }

        public PitTileNeighbours(Tile north, Tile south, Tile east, Tile west) : base(north, south, east, west)
        {
        }
    }
}