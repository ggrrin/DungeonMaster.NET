using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public abstract  class SpellFactory<T> : ISpellFactory where T : ISpellFactory, new ()
    {
        public static ISpellFactory Instance { get; } = new T();

        public abstract IEnumerable<SpellSymbol> CastingSequence { get; }

        public abstract  ISpell CastSpell(Tile location, Point startDirection);
    }
}
