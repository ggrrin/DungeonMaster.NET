using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterParser.Tiles
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
