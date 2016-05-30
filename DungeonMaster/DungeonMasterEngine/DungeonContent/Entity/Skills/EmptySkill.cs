using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;

namespace DungeonMasterEngine.DungeonContent.Entity.Skills
{
    public class EmptySkill : ISkill {
        public ISkillFactory Factory { get; }
        public SkillBase BaseSkill { get; }
        public long Experience { get; }
        public long TemporaryExperience { get; }
        public int SkillLevel { get; }
        public int BaseSkillLevel { get; }

        public void AddExperience(int experience)
        {
        }
    }
}