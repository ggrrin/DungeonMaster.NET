using DungeonMasterParser.Tiles;

namespace DungeonMasterParser
{
    public interface ITileCreator<out T>
    {
        T GetTile(DoorTile t);
        T GetTile(FloorTile t);
        T GetTile(PitTile t);
        T GetTile(StairsTile t);
        T GetTile(TeleporterTile t);
        T GetTile(TrickTile t);
        T GetTile(WallTile t);
    }
}
