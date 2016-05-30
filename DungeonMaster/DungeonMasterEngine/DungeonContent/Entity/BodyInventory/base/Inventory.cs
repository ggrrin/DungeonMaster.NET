using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base
{
    internal class Inventory : IInventory
    {
        protected readonly IGrabableItem[] storage;
        public IStorageType Type { get; }
        public IReadOnlyList<IGrabableItem> Storage => storage;

        public Inventory(IStorageType type)
        {
            if (type == null)
                throw new ArgumentNullException();

            if (type.Size < 1)
                throw new ArgumentException("Invalid storage size!");

            Type = type;
            storage = new IGrabableItem[type.Size];
        }

        public virtual IGrabableItem TakeItemFrom(int index)
        {
            if (index >= 0 && index < storage.Length)
            {
                var res = storage[index];
                storage[index] = null;
                return res;
            }
            else
            {
                return null;
            }
        }

        public virtual bool AddItemTo(IGrabableItem item, int index)
        {
            if (index >= 0 && index < storage.Length && storage[index] == null && item.Factory.CanBeStoredIn(Type))
            {
                storage[index] = item;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool AddItem(IGrabableItem item)
        {
            int freeIndex = Array.FindIndex(storage, i => i == null);
            if (freeIndex != -1 && item.Factory.CanBeStoredIn(Type))
            {
                storage[freeIndex] = item;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual IEnumerable<IGrabableItem> AddRange(IEnumerable<IGrabableItem> items)
        {
            var res = new List<IGrabableItem>();
            bool full = false;
            foreach (var i in items)
            {
                if (!full && !AddItem(i))
                {
                    full = true;
                }
                else
                {
                    res.Add(i);
                }
            }
            return res;
        }

        public override string ToString() => Type.GetType().Name;
    }
}