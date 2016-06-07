using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public abstract class Tile<TMessage> : Tile where TMessage : Message
    {
        protected Tile(TileInitializer initializer) : base(initializer) { }

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
                    if (ContentActivated)
                        DeactivateTileContent();
                    else
                        ActivateTileContent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            foreach (var tileSide in Sides)
            {
                tileSide.SendMessage(message);
            }
        }

    }

    public abstract class Tile : IStopable, INeighbourable<Tile>, ITile
    {
        protected Tile(TileInitializer initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(TileInitializer initializer)
        {
            GridPosition = initializer.GridPosition;
            Level = initializer.Level;
            Neighbours = initializer.Neighbours;
            Renderer.Initialize();

            initializer.Initializing -= Initialize;
        }

        public virtual LayoutManager LayoutManager { get; } = new LayoutManager();

        public virtual TileNeighbours Neighbours { get; protected set; }

        INeighbours<Tile> INeighbourable<Tile>.Neighbours => Neighbours;

        public abstract bool IsAccessible { get; }

        public Vector3 Position => new Vector3(GridPosition.X, -LevelIndex, GridPosition.Y);

        public int LevelIndex => Level.LevelIndex;

        public Point GridPosition { get; private set; }

        public DungeonLevel Level { get; private set; }

        public virtual Vector3 StayPoint => Position + new Vector3(0.5f);

        public List<IItem> SubItems { get; }

        public virtual bool ContentActivated { get; protected set; }

        public virtual void ActivateTileContent()
        {
            ContentActivated = true;
            foreach (var i in SubItems.Where(x => x.AcceptMessages))
                ((TextTag)i).Visible = true;
            $"Activating message received at {GridPosition}".Dump();
        }

        public virtual void DeactivateTileContent()
        {
            ContentActivated = false;
            foreach (var i in SubItems.Where(x => x.AcceptMessages))
                ((TextTag)i).Visible = false;
            $"Deactivating message recived at {GridPosition}".Dump();
        }

        public virtual void OnObjectEntering(IItem obj) { }

        public virtual void OnObjectLeaving(IItem obj) { }

        public virtual void OnObjectEntered(IItem obj)
        {
            //SubItems.Add(obj);
            ObjectEntered?.Invoke(this, obj);
        }

        public virtual void OnObjectLeft(IItem obj)
        {
            //SubItems.Remove(obj);
            ObjectLeft?.Invoke(this, obj);
        }

        public event EventHandler<IItem> ObjectEntered;
        public event EventHandler<IItem> ObjectLeft;

        public void Update(GameTime gameTime)
        {
            //foreach (var item in SubItems.ToArray()) //Enable modifing collection
            //{
            //    item.Update(gameTime);
            //}
        }

        public abstract IEnumerable<TileSide> Sides { get; }
        public Renderer Renderer { get; set; }
        public Interactor Interactor { get; set; }
    }


    public class RayTileInteractor<TTile> : Interactor where TTile : ITile
    {
        private Matrix transformation;
        public Tile Tile { get; }

        public RayTileInteractor(Tile tile)
        {
            Tile = tile;
        }

        public override void Initialize()
        {
            transformation = Matrix.CreateTranslation(Tile.Position);
        }

        public override void Interact(ILeader leader, ref Matrix matrix, object param)
        {
            var ray = (Ray)leader.Interactor;
            var res = ray.Intersects(new BoundingBox(Tile.Position, Tile.Position + new Vector3(1f)));

            if (res != null)
            {
                Tile.Renderer.Highlight(500);

                Matrix finalMatrix = transformation*matrix;
                foreach (var tileSide in Tile.Sides)
                {
                    tileSide.Interactor?.Interact(leader, ref finalMatrix, param);
                }
            }
        }
    }

}