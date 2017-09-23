using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent
{
    internal interface ILightSource : IUpdate
    {
        int LightPower { get; }
    }
}