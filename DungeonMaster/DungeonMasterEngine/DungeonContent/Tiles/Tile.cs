using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public abstract class Tile : WorldObject, IStopable
    {
        protected Tile(Vector3 position) : base(position)
        {
            graphicsProviders = new GraphicsCollection();
            graphicsProviders.AddListOfDrawables(SubItems = new List<IItem>());
        }

        public virtual INeighbours Neighbours { get; set; }

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
            $"Activating message received at {GridPosition}".Dump();
        }

        public virtual void DeactivateTileContent()
        {
            ContentActivated = false;
            $"Deactivating message recived at {GridPosition}".Dump();
        }

        public virtual void ExecuteContentActivator(ITileContentActivator contentActivator)
        {
            //throw new InvalidOperationException("Activator not implemented!");
        }


        public virtual void OnObjectEntered(object obj)
        {
            ObjectEntered?.Invoke(this, obj);
        }

        public event EventHandler<object> ObjectEntered;

        public virtual void OnObjectLeft(object obj)
        {
            ObjectLeft?.Invoke(this, obj);
        }

        public event EventHandler<object> ObjectLeft;

        public void Update(GameTime gameTime)
        {
            foreach (var item in SubItems.ToArray())//Enable modifing collection
            {
                item.Update(gameTime);
            }
        }
    }


}