using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{

    public class WaterMiscFactory : MiscItemFactory
    {
        public int WaterValue { get; }
        public WaterMiscFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, IRenderer renderer, int waterValue) : base(name, weight, attackCombo, carryLocation, renderer)
        {
            WaterValue = waterValue;
        }
    }
    public class FoodMiscFactory : MiscItemFactory
    {
        public int FoodValue { get; }
        public FoodMiscFactory( string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, IRenderer renderer, int foodValue) : base(name, weight, attackCombo, carryLocation, renderer)
        {
            FoodValue = foodValue;
        }
    }
}