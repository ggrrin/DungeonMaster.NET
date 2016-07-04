using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class FoodProperty : Property
    {

        public FoodProperty(int value, int maxValue)
        {
            BaseValue = maxValue;
            this.value = value;
        }

        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<FoodProperty>.Instance;
    }
}