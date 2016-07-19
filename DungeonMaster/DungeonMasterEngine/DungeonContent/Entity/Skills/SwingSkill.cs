using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    internal class SwingSkill : HiddenSkill{

        public override ISkillFactory Type => SkillFactory<SwingSkill>.Instance;

        public SwingSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}