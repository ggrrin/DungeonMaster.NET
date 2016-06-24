using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public interface IMessageAcceptor<TMessage> where TMessage : Message
    {
        void AcceptMessage(TMessage message);
    }
}