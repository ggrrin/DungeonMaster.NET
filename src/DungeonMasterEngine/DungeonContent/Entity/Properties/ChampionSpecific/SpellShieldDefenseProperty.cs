using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Properties.ChampionSpecific
{
    public class SpellShieldDefenseProperty : Property
    {
        public override int BaseValue { get; set; }= int.MaxValue;
        public override IPropertyFactory Type => PropertyFactory<SpellShieldDefenseProperty>.Instance;
    }
}