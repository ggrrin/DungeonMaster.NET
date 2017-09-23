using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class DoorRenderer : TextureRenderer
    {
        public DoorRenderer(Texture2D doorTexture) : base(Matrix.CreateScale(2 / 3f) * Matrix.CreateTranslation(new Vector3(0, -1 / 6f, 0.3f)), doorTexture) { }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            base.Render(ref currentTransformation, effect, parameter);
            var finalmatrix = Matrix.CreateRotationY(MathHelper.Pi) * currentTransformation;
            return base.Render(ref finalmatrix, effect, parameter);
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            return false;
        }
    }
}