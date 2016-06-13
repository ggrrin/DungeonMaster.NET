using System.Collections.Generic;
using DungeonMasterEngine.Builders.Initializators;
using DungeonMasterEngine.DungeonContent.Entity.Attacks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories
{
    public class ScrollItemFactory : GrabableItemFactoryBase
    {
        public ScrollItemFactory(string name, int weight, IEnumerable<IAttackFactory> attackCombo, IEnumerable<IStorageType> carryLocation, Texture2D texture) : base(name, weight, attackCombo, carryLocation, texture) {}

        public Scroll Create<TItemInitiator>(TItemInitiator initializator) where TItemInitiator : IScrollInitializer
        {
            return  new Scroll(initializator, this);
        }

        public override IGrabableItem Create()
        {
            return Create(new ScrollInitializator { Text = "Artifical Item" });
        }
    }
}