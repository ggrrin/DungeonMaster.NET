using System.Collections.Generic;
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

        public void SendMessage(Message message)
        {
        }

        public void Interact(ILeader leader, ref Matrix matrix, object param)
        {
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public Renderer Renderer { get; set; }
    }
}