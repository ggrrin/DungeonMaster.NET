using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Magic.Symbols
{
    public interface ISpellSymbol
    {
        string Name { get; }
        IReadOnlyList<int> ManaCostsPerLevels { get; }
        int MaxSupportedLevel { get; }
    }
}