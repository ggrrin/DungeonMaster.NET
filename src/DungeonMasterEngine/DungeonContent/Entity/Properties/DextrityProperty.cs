using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{

    public class DextrityProperty : Property {
        public DextrityProperty(int value)
        {
            BaseValue = this.value = value;
        }

        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<DextrityProperty>.Instance;
    }
}