using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions.Factories
{
    public class MeleeAttackFactory : HumanActionFactoryBase
    {
        public MeleeAttackFactory(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina, int mapDifficulty) : base(name, experienceGain, defenseModifer, hitProbability, damage, fatigue, skillIndex, stamina, mapDifficulty) {}
        public override IAction CreateAction(ILiveEntity actionProvider) => new MeleeAttack(this, actionProvider);
    }
}