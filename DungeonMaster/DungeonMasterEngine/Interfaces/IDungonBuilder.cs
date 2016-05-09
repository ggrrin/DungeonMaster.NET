using Microsoft.Xna.Framework;
using DungeonMasterEngine.DungeonContent;

namespace DungeonMasterEngine.Interfaces
{
    public interface IDungonBuilder
    {
        DungeonLevel GetLevel(int i, Dungeon dungeon, Point? startTile);

    }
}
