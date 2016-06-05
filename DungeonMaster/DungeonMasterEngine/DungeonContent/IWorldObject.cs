using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent
{
    public interface IWorldObject 
    {
        Vector3 Position { get; }
        IRenderer Renderer { get; set; }
        IInteractor Inter { get; set; }
    }
}