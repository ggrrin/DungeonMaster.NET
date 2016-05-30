using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class FoodProperty : Property {
        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<HealthProperty>.Instance;
    }
}