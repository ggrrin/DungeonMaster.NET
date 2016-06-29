namespace DungeonMasterEngine.DungeonContent.Entity.Skills.Base
{
    public interface ISkill
    {
        ISkillFactory Type { get; }
        SkillBase BaseSkill { get; }
        long Experience { get; }
        long TemporaryExperience { get; }
        int SkillLevel { get; }
        int BaseSkillLevel { get; }

        void AddExperience(int experience);
    }
}