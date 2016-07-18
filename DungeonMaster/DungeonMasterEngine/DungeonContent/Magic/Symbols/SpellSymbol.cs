using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Magic.Symbols
{
    public class SpellSymbol : ISpellSymbol
    {
        public string Name { get; }
        public IReadOnlyList<int> ManaCostsPerLevels { get; }
        public int MaxSupportedLevel => ManaCostsPerLevels.Count; 

        public SpellSymbol(string name, IReadOnlyList<int> manaCostsPerLevels)
        {
            Name = name;
            ManaCostsPerLevels = manaCostsPerLevels;
        }
    }
}