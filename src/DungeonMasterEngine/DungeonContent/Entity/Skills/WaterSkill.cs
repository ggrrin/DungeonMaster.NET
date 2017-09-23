using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class WaterSkill : HiddenSkill{

        public override ISkillFactory Type => SkillFactory<WaterSkill>.Instance;

        public WaterSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}