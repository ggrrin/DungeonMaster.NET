using System;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class StairsRenderer : TileRenderer<Stairs>
    {
        private readonly Texture2D wallTexture;
        private readonly Model stairsModel;

        private readonly Matrix blenderMatrix;

        public StairsRenderer(MapDirection upperEntry, MapDirection lowerEntry, Stairs stairs, Texture2D wallTexture) : base(stairs)
        {
            this.wallTexture = wallTexture;

            int mirror = 1;
            int rotation = upperEntry.Index - lowerEntry.Index;
            if (rotation < 0)
                rotation += 4;
            switch (rotation)
            {
                case 1:
                    mirror = 1;
                    stairsModel = Resources.Content.Load<Model>("Models/stairs1");
                    break;
                case 2:
                    mirror = 1;
                    stairsModel = Resources.Content.Load<Model>("Models/stairs");
                    break;
                case 3:
                    mirror = -1;
                    stairsModel = Resources.Content.Load<Model>("Models/stairs1");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }



            blenderMatrix = new Matrix(
            new Vector4(0, 0, 1, 0),
            new Vector4(mirror, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, 0, 1)) *
            Matrix.CreateScale(0.5f) *
            Matrix.CreateRotationY(MathHelper.PiOver2 * (MapDirection.South.Index - upperEntry.Index)) *
            Matrix.CreateTranslation(new Vector3(.5f, -.5f, .5f));
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var newTransformation = base.Render(ref currentTransformation, effect, parameter);
            DrawModel(stairsModel, ref newTransformation, effect);
            return newTransformation;
        }

        public void DrawModel(Model model, ref Matrix transformation, BasicEffect effect)
        {
            var backup = effect.GraphicsDevice.RasterizerState;
            effect.GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            effect.Texture = wallTexture;
            effect.World = blenderMatrix * transformation;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                    part.Effect = effect;

                mesh.Draw();
            }

            effect.GraphicsDevice.RasterizerState = backup;
        }
    }
}