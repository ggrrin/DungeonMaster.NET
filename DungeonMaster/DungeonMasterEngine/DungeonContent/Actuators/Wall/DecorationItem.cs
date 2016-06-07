using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class DecorationItem : IActuatorX
    {
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