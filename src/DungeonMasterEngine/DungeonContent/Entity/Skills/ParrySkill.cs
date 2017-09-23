using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class ParrySkill : HiddenSkill {

        public override ISkillFactory Type => SkillFactory<ParrySkill>.Instance;

        public ParrySkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}