using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class NinjaSkill : SkillBase
    {
        public override ISkillFactory Type => SkillFactory<NinjaSkill>.Instance;
        public override SkillBase BaseSkill => null; 

        protected override void ApplySkills( int majorIncrease, int minorIncrease)
        {
            liveEntity.GetProperty(PropertyFactory<VitalityProperty>.Instance).BaseValue += rand.Next(2) & BaseSkillLevel;
            int stamina = liveEntity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue / 21;
            int skillLevelModifer = BaseSkillLevel * 2;
            liveEntity.GetProperty(PropertyFactory<StrengthProperty>.Instance).BaseValue += majorIncrease; 
            liveEntity.GetProperty(PropertyFactory<DextrityProperty>.Instance).BaseValue += minorIncrease;
            liveEntity.GetProperty(PropertyFactory<HealthProperty>.Instance).BaseValue += skillLevelModifer + rand.Next((skillLevelModifer >> 1) + 1);
            liveEntity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue += stamina + rand.Next((stamina >> 1) + 1);
        }

        public NinjaSkill(ILiveEntity liveEntity) : base(liveEntity) {}
    }
}