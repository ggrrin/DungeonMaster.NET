using System;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    internal class ThrowSkill : HiddenSkill{

        public override ISkillFactory Factory => SkillFactory<ThrowSkill>.Instance;

        public ThrowSkill(IEntity entity, SkillBase baseSkill, int skillLevel) : base(entity, baseSkill, skillLevel) {}
    }
}