using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    internal class ThrowSkill : HiddenSkill{

        public override ISkillFactory Type => SkillFactory<ThrowSkill>.Instance;

        public ThrowSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}