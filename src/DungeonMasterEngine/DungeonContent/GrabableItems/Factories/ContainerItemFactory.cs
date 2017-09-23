using System.Collections.Generic;
using DungeonMasterEngine.Builders.ItemCreators;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Factories
{
    public class ContainerItemFactory : GrabableItemFactoryBase
    {
        public ContainerItemFactory(string name, int weight, IEnumerable<IActionFactory> attackCombo, IEnumerable<IStorageType> carryLocation, ITextureRenderer renderer) : base(name, weight, attackCombo, carryLocation, renderer) {}

        public Container Create<TItemInitializator>(TItemInitializator initializator) where TItemInitializator : IContainerInitializer
        {
            return new Container(initializator, this);
        }

        public override IGrabableItem CreateItem()
        {
            return Create(new ContainerInitializer {content = new IGrabableItem[0]});
        }
    }
}