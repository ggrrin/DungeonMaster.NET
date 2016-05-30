using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class HealthProperty : Property
    {
        public HealthProperty(int value)
        {
            BaseValue = this.value = value;
        }

        public override int MaxValue => MathHelper.Min(base.MaxValue, 999);

        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<HealthProperty>.Instance;
    }
}