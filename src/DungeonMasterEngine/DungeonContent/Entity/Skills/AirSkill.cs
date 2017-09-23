using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class AirSkill : HiddenSkill {

        public override ISkillFactory Type => SkillFactory<AirSkill>.Instance;

        public AirSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}