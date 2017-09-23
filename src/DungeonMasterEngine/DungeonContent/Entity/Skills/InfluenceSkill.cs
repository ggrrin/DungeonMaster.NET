using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class InfluenceSkill : HiddenSkill {

        public override ISkillFactory Type => SkillFactory<InfluenceSkill>.Instance;

        public InfluenceSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}