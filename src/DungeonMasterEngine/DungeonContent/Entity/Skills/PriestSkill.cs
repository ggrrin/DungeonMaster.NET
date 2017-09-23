using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class PriestSkill : SkillBase
    {
        public override ISkillFactory Type => SkillFactory<PriestSkill>.Instance;
        public override SkillBase BaseSkill => null;

        protected override void ApplySkills(int majorIncrease, int minorIncrease)
        {
            liveEntity.GetProperty(PropertyFactory<VitalityProperty>.Instance).BaseValue += rand.Next(2); 
            int stamina = liveEntity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue / 25;
            int skillLevelModifer = BaseSkillLevel;
            liveEntity.GetProperty(PropertyFactory<ManaProperty>.Instance).BaseValue += skillLevelModifer +  MathHelper.Min(rand.Next(4), skillLevelModifer - 1);
            skillLevelModifer += (skillLevelModifer + 1) >> 1;
            liveEntity.GetProperty(PropertyFactory<WisdomProperty>.Instance).BaseValue += minorIncrease; 
            liveEntity.GetProperty(PropertyFactory<AntiMagicProperty>.Instance).BaseValue += rand.Next(3); 
            liveEntity.GetProperty(PropertyFactory<HealthProperty>.Instance).BaseValue += skillLevelModifer + rand.Next((skillLevelModifer >> 1) + 1);
            liveEntity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue += stamina + rand.Next((stamina >> 1) + 1);
        }

        public PriestSkill(ILiveEntity liveEntity) : base(liveEntity) {}
    }
}