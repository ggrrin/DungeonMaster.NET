using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class DeffendSkill : HiddenSkill {
        public override ISkillFactory Factory => SkillFactory<DeffendSkill>.Instance;

        public DeffendSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}