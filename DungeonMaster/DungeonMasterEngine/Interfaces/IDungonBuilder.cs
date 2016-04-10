using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;

namespace DungeonMasterEngine.Interfaces
{
    public interface IDungonBuilder
    {
        DungeonLevel GetLevel(int i, Dungeon dungeon, Point? startTile);

    }
}
