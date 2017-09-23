using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public interface IWaterJar
    {
        IGrabableItem Fill(IFactories factories);
    }
}