using System;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public abstract class HiddenSkill : SkillBase 
    {
        private SkillBase baseSkill;

        protected HiddenSkill(ILiveEntity liveEntity, SkillBase baseSkill, int skillLevel) : base(liveEntity)
        {
            Experience = GetExperience(skillLevel);
            BaseSkill = baseSkill;
            BaseSkill.AddInitExperience(Experience);
        }

        private int GetExperience(int skillLevel)
        {
            if (skillLevel == 0)
                return 0;
            const int b = 2;
            int val = 1;

            for (int i = 0; i < skillLevel; i++)
            {
                val *= b;
            }
            return 500 * val;
        }

        public sealed override SkillBase BaseSkill { get; }

        protected sealed override void LevelUp()
        {
            throw new InvalidOperationException();
        }

        protected sealed override void ApplySkills(int majorIncrease, int minorIncrease)
        {
            throw new InvalidOperationException();
        }
    }
}