using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class Alcove : IActuatorX
    {
        protected readonly Stack<IGrabableItem> items;

        public IEnumerable<IGrabableItem> Items => items;

        public Alcove(IEnumerable<IGrabableItem> items)
        {
            this.items = new Stack<IGrabableItem>(items);
        }

        public void AcceptMessage(Message message) { }

        public Renderer Renderer { get; set; }

        public virtual bool Trigger(ILeader leader)
        {
            if (leader.Hand == null && items.Any())
            {
                leader.Hand = items.Pop();
            }
            else if( leader.Hand != null )
            {
                items.Push(leader.Hand);
                leader.Hand = null;
            }
            return true;
        }
    }
}