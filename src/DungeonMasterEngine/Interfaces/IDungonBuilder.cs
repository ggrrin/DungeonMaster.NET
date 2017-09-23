using Microsoft.Xna.Framework;
using DungeonMasterEngine.DungeonContent;

namespace DungeonMasterEngine.Interfaces
{
    public interface IDungonBuilder<in TFactories> where TFactories : IFactories
    {
        DungeonLevel GetLevel(TFactories factories, int level, Point? startTile);

    }
}
