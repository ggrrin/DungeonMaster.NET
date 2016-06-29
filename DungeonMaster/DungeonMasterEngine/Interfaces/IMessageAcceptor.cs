using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.Interfaces
{
    public interface IMessageAcceptor<TMessage> where TMessage : Message
    {
        void AcceptMessage(TMessage message);
    }
}