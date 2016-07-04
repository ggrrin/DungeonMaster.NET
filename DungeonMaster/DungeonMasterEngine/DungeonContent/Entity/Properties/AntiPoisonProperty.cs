using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    public class AntiPoisonProperty : Property
    {
        public AntiPoisonProperty(int value)
        {
            BaseValue = this.value = value;
        }
        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<AntiPoisonProperty>.Instance;

    }
}