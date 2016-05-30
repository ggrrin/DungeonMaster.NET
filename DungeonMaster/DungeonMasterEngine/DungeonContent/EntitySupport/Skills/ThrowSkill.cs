using System;
using DungeonMasterEngine.DungeonContent.EntitySupport.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Attacks
{
    internal class ThrowSkill : SkillBase {
        public ThrowSkill(IEntity entity) : base(entity) {}

        public override ISkillFactory Factory { get; }
        public override SkillBase BaseSkill { get; }

        protected override void ApplySkills(int majorIncrease, int minorIncrease)
        {
            throw new NotImplementedException();
        }
    }
}