using System.Collections.Generic;

namespace DungeonMasterParser.Descriptors
{
    public class FightComboDescriptor
    {
        public int ComboIndex { get; set; }
        public IReadOnlyList<ComboEntry> Actions { get; set; }
    }
}