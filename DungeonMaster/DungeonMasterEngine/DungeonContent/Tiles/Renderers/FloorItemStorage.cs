using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class FloorItemStorage
    {
        private readonly List<IGrabableItem> items;
        public IEnumerable<IGrabableItem> Items => items;

        public ISpace Space { get; }
        public Point RelativeGridPosition => Space.GridPosition * new Point(2) - new Point(1);

        public event EventHandler<IGrabableItem> ItemAdding;
        public event EventHandler<IGrabableItem> ItemRemoving;

        private Small4GroupLayout layout => Small4GroupLayout.Instance;
        public IGroupLayout Layout => layout;

        public FloorItemStorage(Point gridPosition, IEnumerable<IGrabableItem> items)
        {
            Space = layout.GetSpace(gridPosition);
            this.items = new List<IGrabableItem>(items);
            foreach (var item in items)
            {
                item.SetLocationNoEvents(layout.GetSpaceElement(Space, item.Location.Tile));
            }
        }

        public void AddItem(IGrabableItem item, bool triggerEvent = true)
        {
            items.Add(item);

            if (triggerEvent)
                ItemAdding?.Invoke(this, item);
        }

        public IGrabableItem RemoveItem(IGrabableItem item, bool triggerEvent = true)
        {
            if (triggerEvent)
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
                leader.Hand.SetLocationNoEvents(Layout.GetSpaceElement(Space, leader.Location));
                AddItem(leader.Hand);
                leader.Hand = null;
                return true;
            }

            return false;
        }
    }
}