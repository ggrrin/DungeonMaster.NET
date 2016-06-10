using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Attacks
{
    public class HumanAttackFactory : IAttackFactory
    {
        public string Name { get; }
        public int ExperienceGain { get; }
        public int DefenseModifer { get; }
        public int HitProbability { get; }
        public int Damage { get; }
        public int Fatigue { get; }
        public ISkillFactory SkillIndex { get; }
        public int Stamina { get; }

        public int MapDifficulty { get; set; }

        public HumanAttackFactory(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina, int mapDifficulty)
        {
            Name = name;
            ExperienceGain = experienceGain;
            DefenseModifer = defenseModifer;
            HitProbability = hitProbability;
            Damage = damage;
            Fatigue = fatigue;
            SkillIndex = skillIndex;
            Stamina = stamina;
            MapDifficulty = mapDifficulty;
        }

        public IAttack CreateAttackAction(ILiveEntity attackProvider)=> new HumanAttack(this, attackProvider);
    }
}