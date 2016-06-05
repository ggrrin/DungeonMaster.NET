using System.Collections.Generic;
using DungeonMasterEngine.Builders.Initializators;
using DungeonMasterEngine.DungeonContent.Entity.Attacks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
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

        public override IGrabableItem Create()
        {
            return Create(new ContainerInitializer {content = new IGrabableItem[0]});
        }
    }
}