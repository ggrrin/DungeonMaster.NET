using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class FloorItemStorage
    {
        public Point GridPosition { get; }
        private readonly List<IGrabableItem> items;
        public IEnumerable<IGrabableItem> Items => items;

        public event EventHandler<IGrabableItem> ItemAdding;
        public event EventHandler<IGrabableItem> ItemRemoving;

        public FloorItemStorage(Point gridPosition, IEnumerable<IGrabableItem> items)
        {
            GridPosition = gridPosition;
            this.items = new List<IGrabableItem>(items);
        }

        public void AddItem(IGrabableItem item, bool triggerEvent = true)
        {
            items.Add(item);

            if(triggerEvent)
                ItemAdding?.Invoke(this, item);
        }

        public IGrabableItem RemoveItem(IGrabableItem item, bool triggerEvent = true)
        {
            if(triggerEvent)
                ItemRemoving?.Invoke(this, item);

            items.Remove(item);
            return item;
        }

        public bool Trigger(ILeader leader)
        {
            if (leader.Hand == null && items.Any())
            {
                leader.Hand = RemoveItem(items.Last());
                leader.Hand.SetLocationNoEvents(null);
                return true;
            }
            else if (leader.Hand != null)
            {
                leader.Hand.SetLocationNoEvents(leader.Location);
                AddItem(leader.Hand);
                leader.Hand = null;
                return true;
            }

            return false;
        }
    }
}