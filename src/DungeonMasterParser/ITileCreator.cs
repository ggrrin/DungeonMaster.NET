using DungeonMasterParser.Tiles;

namespace DungeonMasterParser
{
    public interface ITileCreator<out T>
    {
        T GetTile(DoorTileData t);
        T GetTile(FloorTileData t);
        T GetTile(PitTileData t);
        T GetTile(StairsTileData t);
        T GetTile(TeleporterTileData t);
        T GetTile(TrickTileData t);
        T GetTile(WallTileData t);
    }
}
