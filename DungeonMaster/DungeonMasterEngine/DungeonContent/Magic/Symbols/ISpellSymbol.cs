using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Magic.Symbols
{
    public interface ISpellSymbol
    {
        string Name { get; }
        IReadOnlyList<int> ManaCostsPerLevels { get; }
        int MaxSupportedLevel { get; }
    }

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