using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class FighterSkill : SkillBase
    {
        public override ISkillFactory Factory => SkillFactory<FighterSkill>.Instance;
        public override SkillBase BaseSkill => null;

        protected override void ApplySkills(int majorIncrease, int minorIncrease)
        {
            liveEntity.GetProperty(PropertyFactory<VitalityProperty>.Instance).BaseValue += rand.Next(2) & BaseSkillLevel;
            int stamina = liveEntity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue >> 4;
            int skillLevelModifer = BaseSkillLevel * 3;
            liveEntity.GetProperty(PropertyFactory<StrengthProperty>.Instance).BaseValue += majorIncrease;
            liveEntity.GetProperty(PropertyFactory<DextrityProperty>.Instance).BaseValue += minorIncrease;
            liveEntity.GetProperty(PropertyFactory<HealthProperty>.Instance).BaseValue += skillLevelModifer + rand.Next((skillLevelModifer >> 1) + 1);
            liveEntity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue += stamina + rand.Next((stamina >> 1) + 1);
        }

        public FighterSkill(ILiveEntity liveEntity) : base(liveEntity) { }
    }
}