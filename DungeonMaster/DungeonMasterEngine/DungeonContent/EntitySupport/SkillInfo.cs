using DungeonMasterEngine.DungeonContent.EntitySupport.Skills;

namespace DungeonMasterEngine.DungeonContent.EntitySupport
{
    struct SkillInfo
    {
        public ISkillFactory Skill;
        public ISkillFactory BasicSkill;
        public bool IsHidden;
        public bool IgnoreTemporaryExperience;
        public bool IgnoreObjectModifiers;

        public SkillInfo(ISkillFactory skill) : this()
        {
            Skill = skill;
        }
    }
}