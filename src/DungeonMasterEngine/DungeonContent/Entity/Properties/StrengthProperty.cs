using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class StrengthProperty : Property {
        public StrengthProperty(int value)
        {
            BaseValue = this.value = value;
        }
        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<StrengthProperty>.Instance;

    }
}