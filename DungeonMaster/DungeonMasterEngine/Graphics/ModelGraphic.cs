using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Graphics.ResourcesProvides;

namespace DungeonMasterEngine.Graphics
{
    public class ModelGraphic : Graphic
    {
        public static Matrix blenderTransformation = new Matrix(
                    new Vector4(0, 0, 1, 0),
                    new Vector4(-1, 0, 0, 0),
                    new Vector4(0, 1, 0, 0),
                    new Vector4(0, 0, 0, 1)) * Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(new Vector3(0.5f));

        private Model model;

        public Model Model
        {
            get { return model; }
            set { model = value; }
        }


        public ResourceProvider Resources => ResourceProvider.Instance;


        public ModelGraphic()
        {
            preTransformation = blenderTransformation;
        }

        public override void Draw(BasicEffect globalEffect)
        {
            var backup = globalEffect.GraphicsDevice.RasterizerState;
            globalEffect.GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };

            globalEffect.World = transformationMatrix;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)                
                    part.Effect = globalEffect;

                mesh.Draw();
            }

            globalEffect.GraphicsDevice.RasterizerState = backup;
        }

    }
}
