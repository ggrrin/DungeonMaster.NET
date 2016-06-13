using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Builders
{
    public class ChampionMirrorRenderer : DecorationRenderer<ChampionDecoration>
    {
        private readonly TextureRenderer faceRenderer;


        public ChampionMirrorRenderer(ChampionDecoration graphics, Texture2D mirror, Texture2D face) : base(mirror, graphics)
        {
            faceRenderer = new TextureRenderer((Matrix.CreateScale(0.25f) * Matrix.CreateTranslation(-Vector3.UnitZ * 0.497f)), face);
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var mat = base.Render(ref currentTransformation, effect, parameter);
            if (Item.ShowChampion)
            {
                return faceRenderer.Render(ref currentTransformation, effect, parameter);
            }
            else
            {
                return mat;
            }
        }
    }
}