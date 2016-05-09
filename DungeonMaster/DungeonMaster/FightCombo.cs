using System.Collections.Generic;

namespace DungeonMasterParser
{
    public class FightCombo
    {
        public int ComboIndex { get; set; }
        public IReadOnlyList<ComboEntry> Actions { get; set; }
    }
}