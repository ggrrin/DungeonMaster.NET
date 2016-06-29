using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class WaterProperty : Property {
        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<WaterProperty>.Instance;
    }
}