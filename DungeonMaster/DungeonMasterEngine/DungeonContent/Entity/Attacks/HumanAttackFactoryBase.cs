using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Attacks
{
    public abstract class HumanAttackFactoryBase : IAttackFactory
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

        public HumanAttackFactoryBase(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina, int mapDifficulty)
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

        public abstract IAttack CreateAttackAction(ILiveEntity attackProvider);
    }

    public class SwingAttackFactory : HumanAttackFactoryBase
    {
        public SwingAttackFactory(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina, int mapDifficulty) : base(name, experienceGain, defenseModifer, hitProbability, damage, fatigue, skillIndex, stamina, mapDifficulty) {}
        public override IAttack CreateAttackAction(ILiveEntity attackProvider) => new SwingAttack(this, attackProvider);
    }

    public class MeleeAttackFactory : HumanAttackFactoryBase
    {
        public MeleeAttackFactory(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina, int mapDifficulty) : base(name, experienceGain, defenseModifer, hitProbability, damage, fatigue, skillIndex, stamina, mapDifficulty) {}
        public override IAttack CreateAttackAction(ILiveEntity attackProvider) => new MeleeAttack(this, attackProvider);
    }


}