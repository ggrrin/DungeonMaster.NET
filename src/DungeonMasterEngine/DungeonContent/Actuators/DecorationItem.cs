using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class DecorationItem : IActuatorX
    {
        public void AcceptMessage(Message message)
        {
        }

        public void Interact(ILeader leader, ref Matrix matrix, object param)
        {
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public IRenderer Renderer { get; set; }

        public bool Trigger(ILeader leader)
        {
            return false;
        }
    }
}