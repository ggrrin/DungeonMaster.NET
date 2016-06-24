using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public interface ISpellFactory
    {
        IEnumerable<SpellSymbol> CastingSequence { get; }
        ISpell CastSpell(ITile location, MapDirection startDirection);
    }
}