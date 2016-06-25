using System;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public abstract class HumanAttack : IAction
    {
        protected static readonly Random rand = new Random();
        protected readonly HumanActionFactoryBase factoryBase;
        protected readonly ILiveEntity attackProvider;

        protected HumanAttack(HumanActionFactoryBase factoryBase, ILiveEntity attackProvider)
        {
            this.factoryBase = factoryBase;
            this.attackProvider = attackProvider;
        }

        protected abstract void PerformAttack(MapDirection direction, ref int delay);

        protected ILiveEntity GetAccesibleEnemies(MapDirection partyDirection)
        {
            //TODO rework using rectangle intersection
            var targetTile = attackProvider.Location.Tile.Neighbours.GetTile(partyDirection);
            var enemy = targetTile?.LayoutManager.Entities.Where(e => attackProvider.RelationManager.IsEnemy(e.RelationManager.RelationToken)) //todo or otherwise
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

        public async void ApplyAttack(MapDirection direction)
        {
            var delay = factoryBase.Fatigue;
            var requiredSkill = attackProvider.GetSkill(factoryBase.SkillIndex);
            var requiredStamina = factoryBase.Stamina + rand.Next(2);

            PerformAttack(direction, ref delay);

            //AfterSwitch
            if (delay > 0)
            {
                await Task.Delay(delay);
            }
            if (requiredStamina > 0)
            {
                attackProvider.GetProperty(PropertyFactory<StaminaProperty>.Instance).Value -= requiredStamina;
            }
            if (factoryBase.ExperienceGain > 0)
            {
                requiredSkill.AddExperience(factoryBase.ExperienceGain);
            }
        }

    }
}