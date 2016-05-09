using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Player;
using DungeonMasterEngine.Graphics.ResourcesProvides;

namespace DungeonMasterEngine.Helpers
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        CubeGraphic c;

        FreeLookCamera f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //IsFixedTimeStep = false;
            //TargetElapsedTime = new TimeSpan(10000);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        // Set the 3D model to draw.
        Model myModel;
        Texture2D texture;

        // The aspect ratio determines how to scale 3d to 2d projection.
        float aspectRatio;

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            InitializeEffect();
            myModel = Content.Load<Model>("Models/stairs1");
            myModel1 = Content.Load<Model>("Models/stairs");
            texture = Content.Load<Texture2D>("Textures/wall");
            aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width /
            (float)graphics.GraphicsDevice.Viewport.Height;

            ResourceProvider.Instance.Initialize(GraphicsDevice, Content);
            c = new CubeGraphic { Texture = texture, DrawFaces = CubeFaces.Back, Position = Vector3.Zero, Scale = new Vector3(1) };

            f = new FreeLookCamera(this);
            f.Position = new Vector3(0, 0, 5);
            f.ForwardDirection = -Vector3.UnitZ;
            Components.Add(f);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);

            Effect.View = f.View;
            Effect.Projection = f.Projection;



            base.Update(gameTime);
        }

        BasicEffect Effect;
        private void InitializeEffect()
        {
            Effect = new BasicEffect(GraphicsDevice);
            Effect.GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            Effect.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            Effect.World = Matrix.Identity;


            Effect.TextureEnabled = true;

            Effect.EnableDefaultLighting();
            //primitive color
            Effect.AmbientLightColor = new Vector3(0);
            Effect.DiffuseColor = new Vector3(1f);
            Effect.SpecularColor = new Vector3(0);
            Effect.SpecularPower = 0.1f;
            Effect.Alpha = 1f;
            Effect.EmissiveColor = Vector3.UnitX; //pochoden bych dal
            Effect.FogColor = Vector3.Zero;
            Effect.FogEnabled = false;
            Effect.FogStart = 0;
            Effect.FogEnd = 0;
        }


        // Set the position of the model in world space, and set the rotation.
        Vector3 modelPosition = Vector3.Zero;
        float modelRotation = 0.0f;
        private Model myModel1;

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawModel(myModel);
            DrawModel(myModel1);

            base.Draw(gameTime);
        }



        private void DrawModel(Model myModel)
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                //mesh.Effects = new ModelEffectCollection(new List<Effect> { Effect });

                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = Effect;
                }

                var world = new Matrix(
                    new Vector4(0, 0, 1, 0),
                    new Vector4(-1, 0, 0, 0),
                    new Vector4(0, 1, 0, 0),
                    new Vector4(0, 0, 0, 1));



                Effect.World = world * Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(new Vector3(0.5f)); //world * Matrix.CreateTranslation(new Vector3(1));//transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation);

                mesh.Draw();
            }


            c.Draw(Effect);
        }
    }
}
