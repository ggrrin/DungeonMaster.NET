using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Actions.Factories
{
    public class SwingAttackFactory : HumanActionFactoryBase
    {
        public SwingAttackFactory(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina) : base(name, experienceGain, defenseModifer, hitProbability, damage, fatigue, skillIndex, stamina) {}
        public override IAction CreateAction(ILiveEntity actionProvider) => new SwingAttack(this, actionProvider);
    }
}