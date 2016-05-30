using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class StaminaProperty : Property
    {
        public StaminaProperty(int value)
        {
            BaseValue = this.value = value;
        }

        public override int MaxValue => MathHelper.Min(9999, base.MaxValue);
        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<StaminaProperty>.Instance;
    }
}