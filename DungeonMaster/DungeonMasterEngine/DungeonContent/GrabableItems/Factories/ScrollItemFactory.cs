using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemInitializers;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class ScrollItemFactory : GrabableItemFactoryBase
    {
        public ScrollItemFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, Texture2D texture) : base(name, weight, attackCombo, carryLocation, texture) {}

        public Scroll Create<TItemInitiator>(TItemInitiator initializator) where TItemInitiator : IScrollInitializer
        {
            return  new Scroll(initializator, this);
        }

        public override IGrabableItem Create()
        {
            return Create(new ScrollInitializer { Text = "Artifical Item" });
        }
    }
}