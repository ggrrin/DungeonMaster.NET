using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Potions;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class PotionFactory : GrabableItemFactoryBase
    {
        public virtual EmptyPotionFactory EmptyPotionCreator { get; }

        public PotionFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, IRenderer renderer, EmptyPotionFactory emptyPotionCreator) : base(name, weight, attackCombo, carryLocation, renderer)
        {
            EmptyPotionCreator = emptyPotionCreator;
        }

        public override IGrabableItem CreateItem()
        {
            return new Potion(new PotionInitializer
            {
                PotionPower = 255,//TODO default potoion power ? ???
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