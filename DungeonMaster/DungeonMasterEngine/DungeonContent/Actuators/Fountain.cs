using System;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class Fountain: IActuatorX
    {
        public void AcceptMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public void Interact(ILeader leader, ref Matrix matrix, object param)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public Renderer Renderer { get; set; }

        public bool Trigger(ILeader leader)
        {
            return false;
        }
    }
}