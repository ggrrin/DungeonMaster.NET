using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Magic.Symbols.Factories;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public class OpenDoorSpellFactory : SpellFactory<OpenDoorSpellFactory>
    {
        public override IEnumerable<SpellSymbol> CastingSequence { get; } = new[] { SymbolFactory<ZoSymbol>.Instance.Symbol};

        public override ISpell CastSpell(Tile location, MapDirection startDirection) => new OpenDoorSpell(location, startDirection);
    }
}