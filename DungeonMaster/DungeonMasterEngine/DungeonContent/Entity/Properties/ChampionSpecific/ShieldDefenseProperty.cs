using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties.ChampionSpecific
{
    public class ShieldDefenseProperty : Property
    {
        public override int BaseValue { get; set; } = int.MaxValue;
        public override IPropertyFactory Type => PropertyFactory<ShieldDefenseProperty>.Instance;
    }
}