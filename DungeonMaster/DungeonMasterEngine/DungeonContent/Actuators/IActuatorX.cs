using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public interface IActuatorX : IMessageAcceptor<Message>, IRenderable
    {
    }
}