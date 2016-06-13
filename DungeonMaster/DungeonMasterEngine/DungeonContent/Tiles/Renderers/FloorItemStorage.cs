using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
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
            if(triggerEvent)
                ItemAdding?.Invoke(this, item);

            items.Add(item);
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
                return true;
            }
            else if (leader.Hand != null)
            {
                AddItem(leader.Hand);
                leader.Hand = null;
                return true;
            }

            return false;
        }
    }
}