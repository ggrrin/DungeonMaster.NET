using DungeonMasterEngine.DungeonContent.Tiles.Renderers;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public interface IRenderable
    {
        Renderer Renderer { get; set; }
    }
}