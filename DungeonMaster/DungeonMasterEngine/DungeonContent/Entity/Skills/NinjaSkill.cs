using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class NinjaSkill : SkillBase
    {
        public override ISkillFactory Factory => SkillFactory<NinjaSkill>.Instance;
        public override SkillBase BaseSkill => null; 

        protected override void ApplySkills( int majorIncrease, int minorIncrease)
        {
            entity.GetProperty(PropertyFactory<VitalityProperty>.Instance).BaseValue += rand.Next(2) & BaseSkillLevel;
            int stamina = entity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue / 21;
            int skillLevelModifer = BaseSkillLevel * 2;
            entity.GetProperty(PropertyFactory<StrengthProperty>.Instance).BaseValue += majorIncrease; 
            entity.GetProperty(PropertyFactory<DextrityProperty>.Instance).BaseValue += minorIncrease;
            entity.GetProperty(PropertyFactory<HealthProperty>.Instance).BaseValue += skillLevelModifer + rand.Next((skillLevelModifer >> 1) + 1);
            entity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue += stamina + rand.Next((stamina >> 1) + 1);
        }

        public NinjaSkill(IEntity entity) : base(entity) {}
    }
}