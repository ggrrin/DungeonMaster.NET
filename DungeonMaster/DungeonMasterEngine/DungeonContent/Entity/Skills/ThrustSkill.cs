using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class ThrustSkill : HiddenSkill {

        public override ISkillFactory Factory => SkillFactory<ThrustSkill>.Instance;

        public ThrustSkill(IEntity entity, SkillBase baseSkill, int skillLevel) : base(entity, baseSkill, skillLevel) {}
    }
}