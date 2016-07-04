using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    public class FireShieldDefenseProperty : Property
    {
        public override int BaseValue { get; set; } = int.MaxValue;
        public override IPropertyFactory Type => PropertyFactory<FireShieldDefenseProperty>.Instance;
    }
}