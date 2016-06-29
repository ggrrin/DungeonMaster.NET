using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class DeffendSkill : HiddenSkill {
        public override ISkillFactory Type => SkillFactory<DeffendSkill>.Instance;

        public DeffendSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}