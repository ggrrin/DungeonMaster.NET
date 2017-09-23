using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class ClubSkill : HiddenSkill {

        public override ISkillFactory Type => SkillFactory<ClubSkill>.Instance;

        public ClubSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}