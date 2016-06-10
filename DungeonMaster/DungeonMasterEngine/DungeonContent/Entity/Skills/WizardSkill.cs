using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class WizardSkill : SkillBase
    {
        public override ISkillFactory Factory => SkillFactory<WizardSkill>.Instance;
        public override SkillBase BaseSkill => null; 

        protected override void ApplySkills(int majorIncrease, int minorIncrease)
        {
            liveEntity.GetProperty(PropertyFactory<VitalityProperty>.Instance).BaseValue += rand.Next(2) & BaseSkillLevel;
            int stamina = liveEntity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue >> 5;
            int skillLevelModifer = BaseSkillLevel;
            liveEntity.GetProperty(PropertyFactory<ManaProperty>.Instance).BaseValue += skillLevelModifer + (skillLevelModifer>> 1) + MathHelper.Min(rand.Next(4), skillLevelModifer - 1);
            liveEntity.GetProperty(PropertyFactory<WisdomProperty>.Instance).BaseValue += majorIncrease; 
            liveEntity.GetProperty(PropertyFactory<AntiMagicProperty>.Instance).BaseValue += rand.Next(3); 
            liveEntity.GetProperty(PropertyFactory<HealthProperty>.Instance).BaseValue += skillLevelModifer + rand.Next((skillLevelModifer >> 1) + 1);
            liveEntity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue += stamina + rand.Next((stamina >> 1) + 1);
        }

        public WizardSkill(ILiveEntity liveEntity) : base(liveEntity) {}
    }
}