using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;
using DungeonMasterEngine.Player;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems
{
    public class Container : GrabableItem , IInventory
    {
        private readonly Inventory inventoryImplementation = new Inventory(ChestStorageType.Instance); 

        public  ContainerItemFactory FactoryType { get; }
        public override IGrabableItemFactoryBase Factory => FactoryType; 

        public Container(IContainerInitializer initializer, ContainerItemFactory factory)
        {
            if(initializer.content?.Count > 8)
                throw new ArgumentException("Probably to much items.");

            FactoryType = factory;

            AddRange(initializer.content);
        }

        public IStorageType Type => inventoryImplementation.Type;

        public IReadOnlyList<IGrabableItem> Storage => inventoryImplementation.Storage;

        public IGrabableItem TakeItemFrom(int index) => inventoryImplementation.TakeItemFrom(index);

        public bool AddItemTo(IGrabableItem item, int index) => inventoryImplementation.AddItemTo(item, index);

        public bool AddItem(IGrabableItem item) => inventoryImplementation.AddItem(item);

        public IEnumerable<IGrabableItem> AddRange(IEnumerable<IGrabableItem> items) => inventoryImplementation.AddRange(items);

    }
}