using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public abstract class Tile : WorldObject, IStopable, INeighbourable<Tile>
    {
        protected Tile(Vector3 position) : base(position)
        {
            graphicsProviders = new GraphicsCollection();
            graphicsProviders.AddListOfDrawables(SubItems = new List<IItem>());
        }

        public virtual LayoutManager LayoutManager { get; } = new LayoutManager();

        public virtual TileNeighbours Neighbours { get; set; }

        INeighbours<Tile> INeighbourable<Tile>.Neighbours => Neighbours;

        public abstract bool IsAccessible { get; }

        public Point GridPosition => Position.ToGrid();

        public int LevelIndex => -(int)Position.Y;

        public virtual Vector3 StayPoint => Position + new Vector3(0.5f);

        public List<IItem> SubItems { get; }

        protected GraphicsCollection graphicsProviders;

        public sealed override IGraphicProvider GraphicsProvider => graphicsProviders;

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

        public virtual void ExecuteContentActivator(ITileContentActivator contentActivator)
        {
            //throw new InvalidOperationException("Activator not implemented!");
        }

        public virtual void OnObjectEntering(IItem obj)
        {
            
        }

        public virtual void OnObjectLeaving(IItem obj)
        {
            
        }

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
            foreach (var item in SubItems.ToArray())//Enable modifing collection
            {
                item.Update(gameTime);
            }
        }
    }
}