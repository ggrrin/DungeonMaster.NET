using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Potions;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Misc
{
    public class WaterFlaskMisc : Miscellaneous, IHasEffect, IWaterJar
    {
        public WaterMiscFactory WaterFactory { get; }

        public WaterFlaskMisc(IMiscInitializer initializer, WaterMiscFactory miscItemFactory) : base(initializer, miscItemFactory)
        {
            WaterFactory = miscItemFactory;
        }

        public bool Used => ChargeCount <= 0;
        public string Message => "Water drank.";

        public bool ApplyEffect(ILiveEntity entity)
        {
            if (ChargeCount-- > 0)
            {
                var water = entity.GetProperty(PropertyFactory<WaterProperty>.Instance);
                water.Value += WaterFactory.WaterValuePerCharge;

                return true;
            }
            else
            {
                return false;
            }
        }

        public IGrabableItem GetUsedOutcomeItem(IFactories factories)
        {
            return this;
        }

        public IGrabableItem  Fill(IFactories factories)
        {
            ChargeCount = 4;
            return this;
        }
    }
}