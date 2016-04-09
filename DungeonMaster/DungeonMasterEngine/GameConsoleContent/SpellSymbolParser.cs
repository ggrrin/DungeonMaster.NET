using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols.Factories;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class SpellSymbolParser
    {
        public IEnumerable<ISymbolFactory> SymbolFactores { get; }
        public IEnumerable<ISpellFactory> SpellFactories { get; }

        public SpellSymbolParser(IEnumerable<ISymbolFactory> symbolFactores, IEnumerable<ISpellFactory> spellFactories)
        {
            SymbolFactores = symbolFactores;
            SpellFactories = spellFactories;
        }

        public ISpellFactory ParseSpell(IEnumerable<string> symbolParameters)
        {
            var symbolSequence = symbolParameters
                .Select(x => SymbolFactores.FirstOrDefault(y => string.Equals(x, y.Name, StringComparison.InvariantCultureIgnoreCase))?.Symbol)
                .ToArray();

            return symbolSequence.Any(x => x == null) ? null : SpellFactories.FirstOrDefault(x => x.CastingSequence.SequenceEqual(symbolSequence));
        }
    }
}