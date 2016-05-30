using DungeonMasterEngine.DungeonContent.EntitySupport.Properties;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties.@base;
using DungeonMasterEngine.DungeonContent.EntitySupport.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Skills
{
    public class FighterSkill : SkillBase
    {
        public override ISkillFactory Factory => SkillFactory<FighterSkill>.Instance;
        public override SkillBase BaseSkill => null;

        protected override void ApplySkills(int majorIncrease, int minorIncrease)
        {
            entity.GetProperty(PropertyFactory<VitalityProperty>.Instance).BaseValue += rand.Next(2) & BaseSkillLevel;
            int stamina = entity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue >> 4;
            int skillLevelModifer = BaseSkillLevel * 3;
            entity.GetProperty(PropertyFactory<StrengthProperty>.Instance).BaseValue += majorIncrease;
            entity.GetProperty(PropertyFactory<DextrityProperty>.Instance).BaseValue += minorIncrease;
            entity.GetProperty(PropertyFactory<HealthProperty>.Instance).BaseValue += skillLevelModifer + rand.Next((skillLevelModifer >> 1) + 1);
            entity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue += stamina + rand.Next((stamina >> 1) + 1);
        }

        public FighterSkill(IEntity entity) : base(entity) { }
    }
}