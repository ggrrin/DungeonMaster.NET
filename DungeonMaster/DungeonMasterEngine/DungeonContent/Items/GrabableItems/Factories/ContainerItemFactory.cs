using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.Attacks;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories
{
    public class ContainerItemFactory : GrabableItemFactoryBase
    {
        public ContainerItemFactory(string name, float weight, IEnumerable<IAttackFactory> attackCombo, IEnumerable<IStorageType> carryLocation) : base(name, weight, attackCombo, carryLocation) {}

        public Container Create<TItemInitializator>(TItemInitializator initializator) where TItemInitializator : IContainerInitializer
        {
            return new Container(initializator, this);
        }
    }
}