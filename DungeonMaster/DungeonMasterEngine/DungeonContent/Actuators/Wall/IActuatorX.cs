using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public interface IActuatorX : IMessageAcceptor<Message>, IRenderable, ITriggerable
    {
    }
}