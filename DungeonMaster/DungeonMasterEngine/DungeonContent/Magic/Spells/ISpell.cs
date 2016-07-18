using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser.Enums;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public interface ISpell
    {
        void Run(ILiveEntity caster, MapDirection direction);
    }
}
