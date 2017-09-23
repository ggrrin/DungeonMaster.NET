using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Misc;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class FoodMiscFactory : MiscItemFactory
    {
        public int FoodValue { get; }
        public FoodMiscFactory(int foodValue, string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, ITextureRenderer renderer) : base(name, weight, attackCombo, carryLocation, renderer)
        {
            FoodValue = foodValue;
        }

        public override Miscellaneous Create<TItemInitiator>(TItemInitiator initiator)
        {
            return new FoodMisc(initiator, this);
        }
    }
}