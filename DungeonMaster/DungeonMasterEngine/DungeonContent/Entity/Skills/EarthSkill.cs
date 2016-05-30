using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class EarthSkill : HiddenSkill{

        public override ISkillFactory Factory => SkillFactory<EarthSkill>.Instance;

        public EarthSkill(IEntity entity, SkillBase baseSkill, int skillLevel) : base(entity, baseSkill, skillLevel) {}
    }
}