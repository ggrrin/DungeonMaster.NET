using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public interface ISpell : IMovable<ITile>
    {
        void Run();
    }
}
