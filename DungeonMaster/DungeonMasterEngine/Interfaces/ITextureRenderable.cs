using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.Interfaces
{
    public interface ITextureRenderable : IRenderable
    {
        new ITextureRenderer Renderer { get; set; }
        
    }
}