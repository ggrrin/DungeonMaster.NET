using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class StairsRenderer : Renderer
    {
        private readonly Texture2D wallTexture;

        public StairsRenderer(Stairs stairs, Texture2D wallTexture)
        {
            this.wallTexture = wallTexture;
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            return GetCurrentTransformation(ref currentTransformation);
        }

        public override Matrix GetCurrentTransformation(ref Matrix parentTransformation)
        {
            return Matrix.Identity;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            return false;
        }
    }
}