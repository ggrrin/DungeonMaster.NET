using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions.Factories
{
    public class ThrowActionFactory : HumanActionFactoryBase
    {
        public ThrowActionFactory(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina, int mapDifficulty) : base(name, experienceGain, defenseModifer, hitProbability, damage, fatigue, skillIndex, stamina, mapDifficulty) {}

        public override IAction CreateAction(ILiveEntity actionProvider)
        {
            return new ThrowAttakc(this,actionProvider, ActionHandStorageType.Instance);
        }
    }
}