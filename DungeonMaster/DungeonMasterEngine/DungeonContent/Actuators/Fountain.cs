using System;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class Fountain: IActuatorX
    {
        public void AcceptMessage(Message message)
        {
        }


        public IRenderer Renderer { get; set; }

        public bool Trigger(ILeader leader)
        {
            var jar = leader.Hand as IWaterJar;
            if (jar != null)
            {
                jar.Fill();
                return true;
            }
            return false;
        }
    }

    public interface IWaterJar
    {
        IGrabableItem Fill();
    }
}