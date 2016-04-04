using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Items;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.GameConsoleContent;

namespace DungeonMasterEngine
{
    public abstract class Tile : WorldObject, IStopable
    {
        public Tile(Vector3 position) : base(position)
        {
            graphicsProviders = new GraphicsCollection();
            graphicsProviders.AddListOfDrawables(SubItems = new List<Item>());
        }

        public virtual INeighbours Neighbours { get; set; }

        public abstract bool IsAccessible { get; }

        public Point GridPosition { get { return Position.ToGrid(); } }

        public int LevelIndex { get { return -(int)Position.Y; } }

        public virtual Vector3 StayPoint { get { return Position + new Vector3(0.5f); } }

        public List<Item> SubItems { get; }

        protected GraphicsCollection graphicsProviders;

        public sealed override IGraphicProvider GraphicsProvider => graphicsProviders;

        public bool ContentActivated { get; private set; }

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
            
    }


}