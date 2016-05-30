namespace DungeonMasterEngine.DungeonContent.EntitySupport.Skills.@base
{
    public interface ISkill
    {
        ISkillFactory Factory { get; }
        SkillBase BaseSkill { get; }
        long Experience { get; }
        long TemporaryExperience { get; }
        int SkillLevel { get; }
        int BaseSkillLevel { get; }

        void AddExperience(int experience);
    }
}