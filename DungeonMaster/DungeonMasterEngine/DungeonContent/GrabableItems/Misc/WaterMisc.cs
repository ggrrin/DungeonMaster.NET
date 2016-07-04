using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Potions;

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

        public bool ApplyEffect(ILiveEntity entity)
        {
            if (ChargeCount-- > 0)
            {
                var water = entity.GetProperty(PropertyFactory<WaterProperty>.Instance);
                water.Value += 800;

                return true;
            }
            else
            {
                return false;
            }
        }

        public IGrabableItem  Fill()
        {
            ChargeCount = 4;
            return this;
        }
    }
}