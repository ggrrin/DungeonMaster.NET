using DungeonMasterEngine.DungeonContent.EntitySupport.Properties.@base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Properties
{
    internal class HealthProperty : Property
    {
        public override int MaxValue => MathHelper.Min(base.MaxValue, 999);

        public override int BaseValue { get; set; }
        public override IPropertyFactory Type { get; }
    }
}