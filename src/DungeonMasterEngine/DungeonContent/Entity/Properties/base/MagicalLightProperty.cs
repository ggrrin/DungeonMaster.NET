namespace DungeonMasterEngine.DungeonContent.Entity.Properties.Base
{
    public class MagicalLightProperty: Property
    {
        public override int BaseValue { get; set; } = int.MaxValue;
        public override IPropertyFactory Type => PropertyFactory<MagicalLightProperty>.Instance;
    }
}