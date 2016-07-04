using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Potions;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class DrinkablePotionFactory<TPotion> : PotionFactory where TPotion : DrinkablePotion, new()
    {
        public DrinkablePotionFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, IRenderer renderer, EmptyPotionFactory emptyPotionFactory) : base(name, weight, attackCombo, carryLocation, renderer,emptyPotionFactory) { }

        public override IGrabableItem Create(IPotionInitializer initializer)
        {
            initializer.Factory = this;
            var res = new TPotion();
            res.Initialize(initializer);
            return res;
        }

    }
}