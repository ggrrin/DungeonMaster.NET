using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Magic.Spells;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;

namespace DungeonMasterEngine.DungeonContent.Magic
{
    public interface ISpellCastingManager
    {
        IEnumerable<ISpellSymbol> CurrentCastSequence { get; }
        bool TryBeginCastSpell(IPowerSymbol powerSymbol);
        void ClearCastingSequence();
        void RemoveSymbol();
        bool TryCastSpell(IEnumerable<ISpellFactory<ISpell>> spellFactories);
        bool TryCastSymbol(ISpellSymbol symbol);
    }
}