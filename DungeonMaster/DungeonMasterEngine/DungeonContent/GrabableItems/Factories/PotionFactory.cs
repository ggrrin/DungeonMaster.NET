using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemCreators;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Potions;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class PotionFactory : GrabableItemFactoryBase
    {

        public PotionFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, ITextureRenderer renderer) : base(name, weight, attackCombo, carryLocation, renderer)
        { }

        public override IGrabableItem CreateItem()
        {
            return new Potion(new PotionInitializer
            {
                PotionPower = 10,//TODO default potoion power ? ???
                Factory = this
            });
        }

        public virtual IGrabableItem Create(IPotionInitializer initializator)
        {
            initializator.Factory = this;
            return new Potion(initializator);
        }
    }
}