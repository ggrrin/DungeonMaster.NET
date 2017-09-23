namespace DungeonMasterEngine.DungeonContent.Entity.Skills.Base
{
    public interface ISkill
    {
        ISkillFactory Type { get; }
        SkillBase BaseSkill { get; }
        long Experience { get; }
        int TemporaryExperience { get; set; }
        int SkillLevel { get; }
        int BaseSkillLevel { get; }

        void AddExperience(int experience);
    }
}