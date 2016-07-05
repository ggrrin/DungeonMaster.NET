using System.Linq;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actions
{
    public abstract class HumanAttack<TFactory> : AttackBase<TFactory> where TFactory : HumanActionFactoryBase 
    {
        protected readonly ILiveEntity attackProvider;

        protected HumanAttack(TFactory factory, ILiveEntity attackProvider) : base(factory)
        {
            this.attackProvider = attackProvider;
        }

        protected abstract void PerformAttack(MapDirection direction, ref int delay);

        protected ILiveEntity GetAccesibleEnemies(MapDirection partyDirection)
        {
            //TODO rework using rectangle intersection
            var targetTile = attackProvider.Location.Tile.Neighbors.GetTile(partyDirection);
            var enemy = targetTile?.LayoutManager.Entities
                .Where(e => attackProvider.RelationManager.IsEnemy(e.RelationManager.RelationToken)) //todo or otherwise
                .MinObj(c => Vector3.Distance(c.Position, attackProvider.Position));

            if (enemy != null)
            {
                bool isInFirstRow = attackProvider.Location.Space.Sides.Contains(partyDirection);
                if (!isInFirstRow)
                {
                    bool someBodyInFirstRow = null != attackProvider.Location.Tile.LayoutManager.Entities.FirstOrDefault(e => e.Location.Space.Sides.Contains(partyDirection));
                    if (someBodyInFirstRow)
                        return null;
                }
            }
            return enemy;
        }

        public override int ApplyAttack(MapDirection direction)
        {
            var delay = Factory.Fatigue;
            var requiredSkill = attackProvider.GetSkill(Factory.SkillIndex);
            var requiredStamina = Factory.Stamina + rand.Next(2);

            PerformAttack(direction, ref delay);

            if (requiredStamina > 0)
            {
                attackProvider.GetProperty(PropertyFactory<StaminaProperty>.Instance).Value -= requiredStamina;
            }
            if (Factory.ExperienceGain > 0)
            {
                requiredSkill.AddExperience(Factory.ExperienceGain);
            }
            return delay;
        }

    }
}