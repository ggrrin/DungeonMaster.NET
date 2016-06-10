using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public interface ITriggerable
    {
        bool Trigger(ILeader leader);
    }
}