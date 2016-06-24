using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class DoorTileRenderer : TileRenderer<DoorTile>
    {
        private readonly Texture2D doorFrameTexture;
        private readonly TextureRenderer buttonRenderer;

        readonly Matrix blenderMatrix = new Matrix(
            new Vector4(0, 0, 1, 0),
            new Vector4(-1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, 0, 1)) * Matrix.CreateScale(0.5f);

        private readonly Model frameModel;
        private Matrix transformation;

        public DoorTileRenderer(DoorTile tile, Texture2D doorFrameTexture, Texture2D buttonTexture) : base(tile)
        {
            this.doorFrameTexture = doorFrameTexture;
            var buttonTransformation = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(new Vector3(0.4f, -0.1f, 0.51f));
            buttonRenderer = new TextureRenderer(buttonTransformation, buttonTexture);
            frameModel = ResourceProvider.Instance.Content.Load<Model>("Models/outterDoor");
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            Matrix finalTransformation = transformation * GetCurrentTransformation(ref currentTransformation);
            base.Render(ref currentTransformation, effect, parameter);

            if (!Tile.ContentActivated)
                Tile.Door.Renderer.Render(ref finalTransformation, effect, parameter);

            DrawModel(frameModel, ref finalTransformation, effect);

            if (Tile.HasButton)
            {
                buttonRenderer.Render(ref finalTransformation, effect, parameter);
                Matrix backButtonReversed = Matrix.CreateRotationY(MathHelper.Pi) * finalTransformation;
                buttonRenderer.Render(ref backButtonReversed, effect, parameter);
            }
            return finalTransformation;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            if (base.Interact(leader, ref currentTransformation, param))
                return true;

            if (Tile.HasButton)
            {
                var finalMatrix = transformation * GetCurrentTransformation(ref currentTransformation);
                Matrix backButtonReversed = Matrix.CreateRotationY(MathHelper.Pi) * finalMatrix;
                if (buttonRenderer.Interact(leader, ref finalMatrix, param) || buttonRenderer.Interact(leader, ref backButtonReversed, param))
                {
                    Tile.AcceptMessage(new Message(MessageAction.Toggle, MapDirection.North));
                    return true;
                }
            }
            return false;
        }

        public override void Initialize()
        {
            base.Initialize();
            Matrix rotation;
            base.GetTransformation(Tile.Direction, out rotation);
            transformation = Matrix.CreateScale(new Vector3(1, 1, 1 / 4f)) *
                             rotation * Matrix.CreateTranslation(new Vector3(0.5f));
        }

        public void DrawModel(Model model, ref Matrix transformation, BasicEffect effect)
        {
            var backup = effect.GraphicsDevice.RasterizerState;
            effect.GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            effect.Texture = doorFrameTexture;
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