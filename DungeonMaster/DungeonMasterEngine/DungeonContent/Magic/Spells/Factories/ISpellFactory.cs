using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public interface ISpellFactory
    {
        IEnumerable<SpellSymbol> CastingSequence { get; }
        ISpell CastSpell(Tile location, Point startDirection);
    }
}