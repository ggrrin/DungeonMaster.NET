using System;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Attacks
{
    public class ComboAttackFactory : IAttackFactory
    {
        public bool UseCharges { get; }
        public int MinimumSkillLevel { get; }
        public HumanAttackFactoryBase FightAction { get; }

        public ComboAttackFactory(bool useCharges, int minimumSkillLevel, HumanAttackFactoryBase fightAction)
        {
            UseCharges = useCharges;
            MinimumSkillLevel = minimumSkillLevel;
            FightAction = fightAction;
        }

        public IAttack CreateAttackAction(ILiveEntity attackProvider)
        {
            if(attackProvider == null)
                throw new ArgumentNullException();

            if (MinimumSkillLevel <= attackProvider.GetSkill(FightAction.SkillIndex).SkillLevel)
                return FightAction.CreateAttackAction(attackProvider);
            else
                return null;
        }

        public override string ToString()
        {
            return FightAction?.Name ?? "[null]";
        }
    }
}