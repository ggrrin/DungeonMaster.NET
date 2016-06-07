using System;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class DecorationRenderer<TDecoration> : Renderer
    {
        public Texture2D DecorationTexture { get; }

        public TDecoration Item { get; }

        public DecorationResource Resource => DecorationResource.Instance;

        private readonly Matrix transformation;

        public DecorationRenderer(Texture2D decorationTexture, TDecoration item)
        {
            DecorationTexture = decorationTexture;
            Item = item;
            transformation = Matrix.CreateTranslation(-Vector3.UnitZ * 0.48f);
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            Matrix finalTransformation = transformation * currentTransformation;
            if (DecorationTexture != null)
                RenderDecoration(effect, ref finalTransformation);
            return finalTransformation;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            Matrix finalTransformationinerse = Matrix.Invert(transformation * currentTransformation);
            var ray = (Ray)leader.Interactor;

            var res = Resource.BoundingBox.Intersects(ray.Transform(ref finalTransformationinerse));

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
            effect.Texture = DecorationTexture;
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