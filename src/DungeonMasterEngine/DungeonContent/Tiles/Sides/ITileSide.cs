using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Tiles.Sides
{
    public interface ITileSide : IRenderable, IMessageAcceptor<Message>
    {
        
    }
}