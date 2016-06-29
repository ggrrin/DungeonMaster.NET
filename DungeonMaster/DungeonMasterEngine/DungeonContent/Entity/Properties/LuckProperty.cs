using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class LuckProperty : Property {
        public LuckProperty(int value)
        {
            BaseValue = this.value = value;
        }

        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<LuckProperty>.Instance;
    }
}