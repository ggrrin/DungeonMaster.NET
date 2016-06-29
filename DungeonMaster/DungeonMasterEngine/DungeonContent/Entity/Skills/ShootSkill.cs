using System;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    internal class ShootSkill : HiddenSkill{

        public override ISkillFactory Type => SkillFactory<ShootSkill>.Instance;

        public ShootSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity, baseSkill, skillLevel) {}
    }
}