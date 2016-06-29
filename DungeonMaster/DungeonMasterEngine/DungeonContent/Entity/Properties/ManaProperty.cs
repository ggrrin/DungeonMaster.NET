using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class ManaProperty : Property
    {
        public ManaProperty(int value)
        {
            BaseValue = this.value = value;
        }
        public override int MaxValue => MathHelper.Min(base.MaxValue, 900);
        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<ManaProperty>.Instance;
    }
}