using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Potions;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class EmptyPotionFactory : PotionFactory
    {
        public override EmptyPotionFactory EmptyPotionCreator { get; }
        public DrinkablePotionFactory<WaterPotion> WaterCreator { get; set; }

        public EmptyPotionFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, IRenderer renderer) : base(name, weight, attackCombo, carryLocation, renderer, null)
        {
            EmptyPotionCreator = this;
        }

        public EmptyPotion Create()
        {
            return new EmptyPotion(new PotionInitializer { Factory = this });
        }

    }
}