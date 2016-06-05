using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public abstract class Tile<TMessage> : Tile where TMessage : Message
    {
        protected Tile(Vector3 position) : base(position) {}

        public abstract override bool IsAccessible { get; }

        public virtual void AcceptMessage(TMessage message)
        {
            switch (message.Action)
            {
                case MessageAction.Set:
                    ActivateTileContent();
                    break;
                case MessageAction.Clear:
                    DeactivateTileContent();
                    break;
                case MessageAction.Toggle:
                    if(ContentActivated)
                        DeactivateTileContent();
                    else
                        ActivateTileContent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public abstract class Tile : WorldObject, IStopable, INeighbourable<Tile>
    {
        protected Tile(Vector3 position)
        {
            graphicsProviders = new GraphicsCollection();
            graphicsProviders.AddListOfDrawables(SubItems = new List<IItem>());
        }

        public virtual LayoutManager LayoutManager { get; } = new LayoutManager();

        public virtual TileNeighbours Neighbours { get; set; }

        INeighbours<Tile> INeighbourable<Tile>.Neighbours => Neighbours;

        public abstract bool IsAccessible { get; }

        public Point GridPosition => Position.ToGrid();

        public int LevelIndex => -(int) Position.Y;

        public virtual Vector3 StayPoint => Position + new Vector3(0.5f);

        public List<IItem> SubItems { get; }

        protected GraphicsCollection graphicsProviders;

        public sealed override IGraphicProvider GraphicsProvider => graphicsProviders;

        public virtual bool ContentActivated { get; protected set; }

        public virtual void ActivateTileContent()
        {
            ContentActivated = true;
            foreach (var i in SubItems.Where(x => x.AcceptMessages))
                ((TextTag) i).Visible = true;
            $"Activating message received at {GridPosition}".Dump();
        }

        public virtual void DeactivateTileContent()
        {
            ContentActivated = false;
            foreach (var i in SubItems.Where(x => x.AcceptMessages))
                ((TextTag) i).Visible = false;
            $"Deactivating message recived at {GridPosition}".Dump();
        }

        public virtual void ExecuteContentActivator(ITileContentActivator contentActivator)
        {
            //throw new InvalidOperationException("Activator not implemented!");
        }

        public virtual void OnObjectEntering(IItem obj) {}

        public virtual void OnObjectLeaving(IItem obj) {}

        public virtual void OnObjectEntered(IItem obj)
        {
            SubItems.Add(obj);
            ObjectEntered?.Invoke(this, obj);
        }

        public virtual void OnObjectLeft(IItem obj)
        {
            SubItems.Remove(obj);
            ObjectLeft?.Invoke(this, obj);
        }

        public event EventHandler<IItem> ObjectEntered;
        public event EventHandler<IItem> ObjectLeft;

        public void Update(GameTime gameTime)
        {
            foreach (var item in SubItems.ToArray()) //Enable modifing collection
            {
                item.Update(gameTime);
            }
        }

        public IEnumerable<TileSide> Sides { get; }
    }

    public class TileSide
    {
        public CubeFaces Face { get; }

        public Actuator Actuator { get; }

        public IRenderer Renderer { get; set; }
    }

    public class TileWallSideRenderer : TileSideRenderer
    {
        public Texture2D Texture { get; }
        public TileSide Side { get; }

        private Matrix transformation;

        public WallResource Resource => WallResource.Instance;

        public TileWallSideRenderer(Texture2D texture, TileSide side)
        {
            Texture = texture;
            Side = side;

            Matrix rotation;
            GetTransformation(side.Face, out rotation);
            Matrix translation;
            var translationVector = new Vector3(0.5f);
            Matrix.CreateTranslation(ref translationVector, out translation);
            Matrix.Multiply(ref rotation, ref translation, out transformation);
        }

        public override void Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            Matrix finalTransformation;
            Matrix.Multiply(ref transformation, ref currentTransformation, out finalTransformation);
            RenderWall(effect, ref finalTransformation);

            Side.Actuator?.Renderer?.Render(ref finalTransformation, effect, parameter);
        }

        private void RenderWall(BasicEffect effect, ref Matrix finalTransformation)
        {
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

    public abstract class TileSideRenderer : IRenderer
    {
        protected void GetTransformation(CubeFaces faces, out Matrix matrix)
        {
            switch (faces)
            {
                case CubeFaces.Back:
                    matrix = Matrix.Identity;
                    break;
                case CubeFaces.Right:
                    matrix = Matrix.CreateRotationY(-MathHelper.PiOver2);
                    break;
                case CubeFaces.Front:
                    matrix = Matrix.CreateRotationY(MathHelper.Pi);
                    break;
                case CubeFaces.Left:
                    matrix = Matrix.CreateRotationY(-MathHelper.PiOver2);
                    break;
                case CubeFaces.Floor:
                    matrix = Matrix.CreateRotationX(MathHelper.PiOver2);
                    break;
                case CubeFaces.Ceeling:
                    matrix = Matrix.CreateRotationX(-MathHelper.PiOver2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(faces), faces, null);
            }
        }

        public abstract void Render(ref Matrix currentTransformation, BasicEffect effect, object parameter);
    }

    public interface IRenderer
    {
        void Render(ref Matrix currentTransformation, BasicEffect effect, object parameter);
    }

    public class WallResource : BufferResourceProvider
    {
        public new static WallResource Instance { get; } = new WallResource();

        private WallResource()
        {
            var lbf = -Vector3.UnitX/2;
            var rbf = Vector3.UnitX/2;
            var lbc = lbf + Vector3.Up;
            var rbc = rbf + Vector3.Up;

            var vertices = new VertexPositionNormalTexture[]
            {
                new VertexPositionNormalTexture(lbf, Vector3.UnitZ, new Vector2(0, 1)),
                new VertexPositionNormalTexture(rbf, Vector3.UnitZ, new Vector2(1, 1)),
                new VertexPositionNormalTexture(rbc, Vector3.UnitZ, new Vector2(1, 0)),
                new VertexPositionNormalTexture(lbc, Vector3.UnitZ, new Vector2(0, 0)),
            };

            var indeices = new int[]
            {
                0,
                2,
                3,
                0,
                1,
                2,
            };

            IndexBuffer = new IndexBuffer(Device, typeof(int), indeices.Length, BufferUsage.None);
            IndexBuffer.SetData(indeices);

            VertexBuffer = new VertexBuffer(Device, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.None);
            VertexBuffer.SetData(vertices);
        }
    }

    public class TileRenderer : IRenderer
    {
        private Matrix translation;
        public Tile Tile { get; }

        public TileRenderer(Tile tile)
        {
            Tile = tile;
            translation = Matrix.CreateTranslation(tile.Position);
        }

        public void Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var finalTransformation = translation*currentTransformation;
            foreach (var side in Tile.Sides)
                side.Renderer.Render(ref finalTransformation, effect, parameter);
        }
    }

    public enum MessageAction
    {
        Set,
        Clear,
        Toggle
    }

    public class Message
    {
        public MessageAction Action { get; }

        public Message(MessageAction action)
        {
            Action = action;
        }
    }
}