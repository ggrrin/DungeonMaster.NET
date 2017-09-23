using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Actions.Factories
{
    public abstract class HumanActionFactoryBase : IActionFactory
    {
        public string Name { get; }
        public int ExperienceGain { get; }
        public int DefenseModifer { get; }
        public int HitProbability { get; }
        public int Damage { get; }
        public int Fatigue { get; }
        public ISkillFactory SkillIndex { get; }
        public int Stamina { get; }

        public HumanActionFactoryBase(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina)
        {
            Name = name;
            ExperienceGain = experienceGain;
            DefenseModifer = defenseModifer;
            HitProbability = hitProbability;
            Damage = damage;
            Fatigue = fatigue;
            SkillIndex = skillIndex;
            Stamina = stamina;
        }

        public abstract IAction CreateAction(ILiveEntity actionProvider);
    }
}