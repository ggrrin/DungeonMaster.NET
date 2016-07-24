using DungeonMasterEngine.DungeonContent.Actions;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;

namespace DungeonMasterEngine.Builders
{
    public class ActionMocap : HumanActionFactoryBase
    {
        public ActionMocap(string name, int experienceGain, int defenseModifer, int hitProbability, int damage, int fatigue, ISkillFactory skillIndex, int stamina, int mapDifficulty) : base(name, experienceGain, defenseModifer, hitProbability, damage, fatigue, skillIndex, stamina) { }

        public override IAction CreateAction(ILiveEntity actionProvider)
        {
            return null;
        }
    }
}