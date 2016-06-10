using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public interface IMessageAcceptor<TMessage> where TMessage : Message
    {
        void AcceptMessage(TMessage message);
    }
}