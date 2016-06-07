using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public interface IRenderable
    {
        Renderer Renderer { get; set; }
    }
}