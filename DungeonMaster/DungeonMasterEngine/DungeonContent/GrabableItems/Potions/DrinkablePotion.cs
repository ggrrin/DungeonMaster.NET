using System;
using DungeonMasterEngine.Builders.ItemCreators;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Potions
{
    public abstract class DrinkablePotion : Potion, IHasEffect
    {
        public bool Used { get; protected set; } = false;
        public string Message => "Potion successfully drank!";

        protected DrinkablePotion(IPotionInitializer initializer) : base(initializer) { }

        protected DrinkablePotion()
        { }

        public void Initialize(IPotionInitializer initializer)
        {
            InitializePotion(initializer);
        }

        protected void F348_xxxx_INVENTORY_AdjustStatisticCurrentValue(IEntity P715_ps_Champion, IPropertyFactory P716_i_StatisticIndex, int P717_i_ValueDelta)
        {
            if (P717_i_ValueDelta < 0)
                throw new ArgumentException();

            var property = P715_ps_Champion.GetProperty(P716_i_StatisticIndex);
            /* CHANGE2_14_IMPROVEMENT Halve delta if current value is above 120 */
            int A1077_i_CurrentValue;
            if ((A1077_i_CurrentValue = property.Value) > 120)
            {
                P717_i_ValueDelta >>= 1;
                /* CHANGE3_13_IMPROVEMENT Halve again delta if current value is above 150 (maximum value is now 170 so the 180 threshold is too high) */
                if (A1077_i_CurrentValue > 150)
                {
                    P717_i_ValueDelta >>= 1;
                }
                P717_i_ValueDelta++;
            }

            int A1077_i_Delta = MathHelper.Min(P717_i_ValueDelta, 170 - A1077_i_CurrentValue);
            property.Value += A1077_i_Delta;
        }


        //void F349_dzzz_INVENTORY_ProcessCommand70_ClickOnMouth(ILiveEntity entity) => source for children
        public abstract bool ApplyEffect(ILiveEntity entity);

        public IGrabableItem GetUsedOutcomeItem(IFactories factories)
        {
            return factories.PotionFactories[20].Create(new PotionInitializer());
        }
    }
}