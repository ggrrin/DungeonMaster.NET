using DungeonMasterEngine.DungeonContent.EntitySupport.Properties.@base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Properties
{
    internal class StaminaProperty : Property
    {
        public override int MaxValue => MathHelper.Min(9999, base.MaxValue);
        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<StaminaProperty>.Instance;
    }
}