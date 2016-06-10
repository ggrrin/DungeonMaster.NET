using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class FloorItemStorage  : ITriggerable
    {
        public Point GridPosition { get; }
        private readonly Stack<IGrabableItem> items;
        public IEnumerable<IGrabableItem> Items => items;

        public FloorItemStorage(Point gridPosition, IEnumerable<IGrabableItem> items)
        {
            GridPosition = gridPosition;
            this.items = new Stack<IGrabableItem>(items);
        }

        public bool Trigger(ILeader leader)
        {
            if (leader.Hand == null && items.Any())
            {
                leader.Hand = items.Pop();
                return true;
            }
            else if (leader.Hand != null)
            {
                items.Push(leader.Hand);
                leader.Hand = null;
                return true;
            }

            return false;
        }
    }
}