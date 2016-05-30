using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base
{
    public interface IInventory
    {
        IStorageType Type { get; }

        IReadOnlyList<IGrabableItem> Storage { get; }

        IGrabableItem TakeItemFrom(int index);

        bool AddItemTo(IGrabableItem item, int index);

        bool AddItem(IGrabableItem item);

        IEnumerable<IGrabableItem> AddRange(IEnumerable<IGrabableItem> items);
    }
}