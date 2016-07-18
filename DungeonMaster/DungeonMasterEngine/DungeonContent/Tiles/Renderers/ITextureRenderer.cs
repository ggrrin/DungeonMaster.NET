using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public interface ITextureRenderer : IRenderer
    {
        Texture2D Texture { get; }
    }
}