using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Potions;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Misc
{
    public class FoodMisc : Miscellaneous, IHasEffect
    {
        public FoodMiscFactory FoodFactory { get; }

        public FoodMisc(IMiscInitializer initializer, FoodMiscFactory miscItemFactory) : base(initializer, miscItemFactory)
        {
            FoodFactory = miscItemFactory;
        }

        public bool Used { get; protected set; }

        public bool ApplyEffect(ILiveEntity entity)
        {
            if (Used)
                return false;

            var food = entity.GetProperty(PropertyFactory<FoodProperty>.Instance);
            food.Value += FoodFactory.FoodValue;
            return Used = true;
        }
    }
}