using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.Attacks;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories
{
    public class PotionItemFactory : GrabableItemFactoryBase
    {
        public Potion Create<TItemInitializator>(TItemInitializator initializator) where TItemInitializator : IPotionInitializer
        {
            return new Potion(initializator, this);
        }

        public PotionItemFactory(string name, float weight, IEnumerable<IAttackFactory> attackCombo, IEnumerable<IStorageType> carryLocation) : base(name, weight, attackCombo, carryLocation)
        { }
    }
}