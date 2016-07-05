using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class TextureRenderer : Renderer, ITextureRenderer
    {
        public Texture2D Texture { get; }

        public DecorationResource Resource => DecorationResource.Instance;

        private readonly Matrix transformation;

        public TextureRenderer(Matrix transformation, Texture2D decorationTexture)
        {
            Texture = decorationTexture ?? Resource.DefaultTexture;
            Vector3 scale = new Vector3(Texture.Width / (float)Texture.Height, 1, 1);
            this.transformation = Matrix.CreateScale(scale) * transformation;
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            Matrix finalTransformation = GetCurrentTransformation(ref currentTransformation);
            if (Texture != null)
                RenderDecoration(effect, ref finalTransformation);
            return finalTransformation;
        }

        public override Matrix GetCurrentTransformation(ref Matrix parentTransformation) => transformation * parentTransformation;

        //Vector3.Transform( Resource.BoundingBox.Min, GetCurrentTransformation(ref currentTransformation))
        //1,35  -0,15  14,505
        //    X: 1.35
        //    Y: -0.15
        //    Z: 14.505
        //Resource.BoundingBox.Min
        //-0,5  -0,5  -0,05
        //    X: -0.5
        //    Y: -0.5
        //    Z: -0.05
        //Resource.BoundingBox.Max
        //0,5  0,5  0
        //    X: 0.5
        //    Y: 0.5
        //    Z: 0

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            Matrix finalTransformationinerse = Matrix.Invert(GetCurrentTransformation(ref currentTransformation));
            var ray = (Ray)leader.Interactor;

            var transform = ray.Transform(ref finalTransformationinerse);
            var res = Resource.BoundingBox.Intersects(transform);

            if (res != null)
            {
                Highlight(500);
                return true;
            }

            return false;
        }

        private void RenderDecoration(BasicEffect effect, ref Matrix finalTransformation)
        {
            var color = Highlighted ? Color.Orange : Color.White;
            effect.DiffuseColor = color.ToVector3();
            effect.World = finalTransformation;
            effect.Texture = Texture;
            effect.GraphicsDevice.Indices = Resource.IndexBuffer;
            effect.GraphicsDevice.SetVertexBuffer(Resource.VertexBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                effect.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Resource.VertexBuffer.VertexCount, 0, 2);
            }
        }
    }
}