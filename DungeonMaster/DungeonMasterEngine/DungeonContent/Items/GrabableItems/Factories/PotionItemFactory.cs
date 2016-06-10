using System.Collections.Generic;
using DungeonMasterEngine.Builders.Initializators;
using DungeonMasterEngine.DungeonContent.Entity.Attacks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories
{
    public class PotionItemFactory : GrabableItemFactoryBase
    {
        public Potion Create<TItemInitializator>(TItemInitializator initializator) where TItemInitializator : IPotionInitializer
        {
            return new Potion(initializator, this);
        }

        public PotionItemFactory(string name, float weight, IEnumerable<IAttackFactory> attackCombo, IEnumerable<IStorageType> carryLocation, Texture2D texture) : base(name, weight, attackCombo, carryLocation, texture)
        { }

        public override IGrabableItem Create()
        {
            return Create(new PotionInitializer { PotionPower = 255 });
        }
    }
}