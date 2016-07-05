using System;
using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.DungeonContent.Actions.Factories
{
    public class ComboActionFactory : IActionFactory
    {
        public bool UseCharges { get; }
        public int MinimumSkillLevel { get; }
        public HumanActionFactoryBase FightAction { get; }

        public ComboActionFactory(bool useCharges, int minimumSkillLevel, HumanActionFactoryBase fightAction)
        {
            UseCharges = useCharges;
            MinimumSkillLevel = minimumSkillLevel;
            FightAction = fightAction;
        }

        public IAction CreateAction(ILiveEntity actionProvider)
        {
            if(actionProvider == null)
                throw new ArgumentNullException();

            if (MinimumSkillLevel <= actionProvider.GetSkill(FightAction.SkillIndex).SkillLevel)
                return FightAction.CreateAction(actionProvider);
            else
                return null;
        }

        public override string ToString()
        {
            return FightAction?.Name ?? "[null]";
        }
    }
}