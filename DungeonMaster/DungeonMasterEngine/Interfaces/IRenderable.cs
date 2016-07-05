using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.Interfaces
{
    public interface IRenderable
    {
        IRenderer Renderer { get; set; }
    }

    public interface ITextureRenderable : IRenderable
    {
        new ITextureRenderer Renderer { get; set; }
        
    }
}