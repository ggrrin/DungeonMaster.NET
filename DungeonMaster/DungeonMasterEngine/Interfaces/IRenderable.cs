using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.Interfaces
{
    public interface IRenderable
    {
        IRenderer Renderer { get; set; }
    }
}