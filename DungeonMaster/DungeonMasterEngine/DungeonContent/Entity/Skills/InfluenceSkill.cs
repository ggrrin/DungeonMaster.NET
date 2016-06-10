using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class InfluenceSkill : HiddenSkill {

        public override ISkillFactory Factory => SkillFactory<InfluenceSkill>.Instance;

        public InfluenceSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}