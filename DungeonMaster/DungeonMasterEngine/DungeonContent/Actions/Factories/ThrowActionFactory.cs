using DungeonMasterEngine.Builders;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Actions.Factories
{
    public class ThrowActionFactory : HumanActionFactoryBase
    {
        public IRenderersSource Renderers { get; }
        public ThrowActionFactory(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina, IRenderersSource renderers) : base(name, experienceGain, defenseModifer, hitProbability, damage, fatigue, skillIndex, stamina)
        {
            Renderers = renderers;
        }

        public override IAction CreateAction(ILiveEntity actionProvider)
        {
            return new ThrowAttack(this,actionProvider, ActionHandStorageType.Instance);
        }
    }
}