using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class AirSkill : HiddenSkill {

        public override ISkillFactory Factory => SkillFactory<AirSkill>.Instance;

        public AirSkill(IEntity entity, SkillBase baseSkill, int skillLevel) : base(entity, baseSkill, skillLevel) {}
    }
}