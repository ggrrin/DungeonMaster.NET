using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class FoodProperty : Property
    {

        public FoodProperty(int value, int maxValue)
        {
            BaseValue = maxValue;
            this.value = value;
        }

        public override int Value
        {
            get { return value; }
            set
            {
                var prevVal = this.value;
                this.value = MathHelper.Clamp(value, -1024, MaxValue);
                $"{GetType().Name}: {Value} of {MaxValue} ; {this.value - prevVal}".Dump();
                InvokeValueChanged();
            }
        }


        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<FoodProperty>.Instance;
    }
}