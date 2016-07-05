using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class ScrollItemFactory : GrabableItemFactoryBase
    {
        public ScrollItemFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, ITextureRenderer renderer) : base(name, weight, attackCombo, carryLocation, renderer) {}

        public Scroll Create<TItemInitiator>(TItemInitiator initializator) where TItemInitiator : IScrollInitializer
        {
            return  new Scroll(initializator, this);
        }

        public override IGrabableItem CreateItem()
        {
            return Create(new ScrollInitializer { Text = "Artificial Item" });
        }
    }
}