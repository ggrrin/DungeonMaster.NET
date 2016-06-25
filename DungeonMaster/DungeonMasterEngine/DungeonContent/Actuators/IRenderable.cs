using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public interface IRenderable
    {
        IRenderer Renderer { get; set; }
    }
}