using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    internal class Container : GrabableItem , IInventory
    {
        private readonly Inventory inventoryImplementation = new Inventory(ChestStorageType.Instance); 

        public Container(Vector3 position, ICollection<IGrabableItem> content) : base(position)
        {
            if(content?.Count > 8)
                throw new ArgumentException("Probably to much items.");

            AddRange(content);
        }

        public IStorageType Type => inventoryImplementation.Type;

        public IReadOnlyList<IGrabableItem> Storage => inventoryImplementation.Storage;

        public IGrabableItem TakeItemFrom(int index) => inventoryImplementation.TakeItemFrom(index);

        public bool AddItemTo(IGrabableItem item, int index) => inventoryImplementation.AddItemTo(item, index);

        public bool AddItem(IGrabableItem item) => inventoryImplementation.AddItem(item);

        public IEnumerable<IGrabableItem> AddRange(IEnumerable<IGrabableItem> items) => inventoryImplementation.AddRange(items);
    }
}