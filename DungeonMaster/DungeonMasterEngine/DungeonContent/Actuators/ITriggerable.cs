using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public interface ITriggerable
    {
        bool Trigger(ILeader leader);
    }
}