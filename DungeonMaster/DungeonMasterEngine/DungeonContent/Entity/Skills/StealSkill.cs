using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class StealSkill : HiddenSkill {

        public override ISkillFactory Factory => SkillFactory<StealSkill>.Instance;

        public StealSkill(IEntity entity, SkillBase baseSkill, int skillLevel) : base(entity, baseSkill, skillLevel) {}
    }
}