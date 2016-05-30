using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class PriestSkill : SkillBase
    {
        public override ISkillFactory Factory => SkillFactory<PriestSkill>.Instance;
        public override SkillBase BaseSkill => null;

        protected override void ApplySkills(int majorIncrease, int minorIncrease)
        {
            entity.GetProperty(PropertyFactory<VitalityProperty>.Instance).BaseValue += rand.Next(2); 
            int stamina = entity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue / 25;
            int skillLevelModifer = BaseSkillLevel;
            entity.GetProperty(PropertyFactory<ManaProperty>.Instance).BaseValue += skillLevelModifer +  MathHelper.Min(rand.Next(4), skillLevelModifer - 1);
            skillLevelModifer += (skillLevelModifer + 1) >> 1;
            entity.GetProperty(PropertyFactory<WisdomProperty>.Instance).BaseValue += minorIncrease; 
            entity.GetProperty(PropertyFactory<AntiMagicProperty>.Instance).BaseValue += rand.Next(3); 
            entity.GetProperty(PropertyFactory<HealthProperty>.Instance).BaseValue += skillLevelModifer + rand.Next((skillLevelModifer >> 1) + 1);
            entity.GetProperty(PropertyFactory<StaminaProperty>.Instance).BaseValue += stamina + rand.Next((stamina >> 1) + 1);
        }

        public PriestSkill(IEntity entity) : base(entity) {}
    }
}