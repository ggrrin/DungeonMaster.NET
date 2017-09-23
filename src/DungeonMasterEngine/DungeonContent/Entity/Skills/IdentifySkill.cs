using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class IdentifySkill : HiddenSkill {

        public override ISkillFactory Type => SkillFactory<IdentifySkill>.Instance;

        public IdentifySkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}