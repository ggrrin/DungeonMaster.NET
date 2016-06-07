using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public interface IMessageAcceptor
    {
        void SendMessage(Message message);
    }
}