using System;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterParser.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class DoorTile : DoorTile<Message>
    {
        public DoorTile(DoorInitializer initializer) : base(initializer) { }
    }

    public class DoorTile<TMessage> : FloorTile<TMessage> where TMessage : Message
    {
        public Door Door { get; private set; }

        public bool HasButton { get; private set; }
        public MapDirection Direction { get; private set; }

        public override bool ContentActivated
        {
            get { return Door.Open; }
            protected set { Door.Open = value; }
        }

        public override bool IsAccessible => ContentActivated;

        public DoorTile(DoorInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(DoorInitializer initializer)
        {
            Direction = initializer.Direction;
            Door = initializer.Door;
            HasButton = initializer.HasButton;

            initializer.Initializing -= Initialize;
        }
    }


    public class TeleportFloorTileSideRenderer : FloorTileSideRenderer<FloorTileSide>
    {
        private readonly TextureRenderer teleport;

        public TeleportFloorTileSideRenderer(FloorTileSide tileSide, Texture2D wallTexture, Texture2D teleportTexture) : base(tileSide, wallTexture, teleportTexture)
        {
            var identity = Matrix.CreateTranslation(new Vector3(0,0, -0.499f));
            this.teleport = new TextureRenderer(identity, teleportTexture);
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var matrix = base.Render(ref currentTransformation, effect, parameter);

            //if(Tile.Visible)
            teleport.Render(ref matrix, effect, parameter);

            return matrix;
        }
    }


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
            Matrix finalTransformation = transformation * base.Render(ref currentTransformation, effect, parameter);

            DrawModel(frameModel, ref finalTransformation, effect);
            if(!Tile.ContentActivated)
                Tile.Door.Renderer.Render(ref finalTransformation, effect, parameter);

            if (Tile.HasButton)
            {
                buttonRenderer.Render(ref finalTransformation, effect, parameter);
                Matrix backButtonReversed = Matrix.CreateRotationY(MathHelper.Pi)*finalTransformation;
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
                var finalMatrix = transformation*GetCurrentTransformation(ref currentTransformation);
                Matrix backButtonReversed = Matrix.CreateRotationY(MathHelper.Pi)*finalMatrix;
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
