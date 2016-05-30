using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class VitalityProperty : Property
    {
        public VitalityProperty(int value)
        {
            BaseValue = this.value = value;
        }

        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<VitalityProperty>.Instance;

    }
}