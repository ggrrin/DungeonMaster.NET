using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public interface ISpell
    {
        void Run(ILiveEntity caster, MapDirection direction);
    }
}
