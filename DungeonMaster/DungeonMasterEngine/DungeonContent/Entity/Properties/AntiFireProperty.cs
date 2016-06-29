using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties
{
    internal class AntiFireProperty : Property {
        public AntiFireProperty(int value)
        {
            BaseValue = this.value = value;
        }
        public override int BaseValue { get; set; }
        public override IPropertyFactory Type => PropertyFactory<AntiFireProperty>.Instance;
    }
}