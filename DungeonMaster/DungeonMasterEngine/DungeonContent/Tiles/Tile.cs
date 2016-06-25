using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public abstract class Tile<TMessage> : Tile, IMessageAcceptor<TMessage> where TMessage : Message
    {
        protected Tile(TileInitializer initializer) : base(initializer) { }

        public abstract override bool IsAccessible { get; }

        public sealed override void AcceptMessageBase(Message message)
        {

            if(message is TMessage)
                AcceptMessage(tMessage: (TMessage)message);
        }

        public virtual void AcceptMessage(TMessage tMessage)
        {
            base.AcceptMessageBase(tMessage);
        }

    }

    public abstract class Tile :  ITile
    {
        public abstract IEnumerable<object> SubItems { get; } 

        protected Tile(TileInitializer initializer)
        {
            initializer.Initializing += Initialize;
            initializer.Initialized += AfterInitialized;
        }

        private void AfterInitialized(TileInitializer initializer)
        {
            Renderer?.Initialize();
            foreach (var liveEntity in initializer.Creatures)
            {
                OnObjectEntered(liveEntity);
                liveEntity.Position = liveEntity.Location.StayPoint;
            }

            initializer.Initialized -= AfterInitialized;
        }

        private void Initialize(TileInitializer initializer)
        {
            GridPosition = initializer.GridPosition;
            Level = initializer.Level;
            Neighbours = initializer.Neighbours;


            initializer.Initializing -= Initialize;
        }

        public virtual LayoutManager<ILiveEntity> LayoutManager { get; } = new LayoutManager<ILiveEntity>();

        public virtual TileNeighbours Neighbours { get; protected set; }

        INeighbours<ITile> INeighbourable<ITile>.Neighbours => Neighbours;

        public abstract bool IsAccessible { get; }
        public virtual bool CanFlyItems => true;
        public virtual bool IsTransparent => true;

        public Vector3 Position => new Vector3(GridPosition.X, -LevelIndex, GridPosition.Y);

        public int LevelIndex => Level.LevelIndex;

        public Point GridPosition { get; private set; }

        public DungeonLevel Level { get; private set; }

        public virtual Vector3 StayPoint => Position + new Vector3(0.5f);

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

        public virtual void OnObjectEntered(object localizable)
        {
            ObjectEntered?.Invoke(this, localizable);
        }

        public virtual void OnObjectLeft(object localizable)
        {
            ObjectLeft?.Invoke(this, localizable);
        }

        public virtual void OnObjectEntering(object localizable) { }

        public virtual void OnObjectLeaving(object localizable) { }

        public event EventHandler<object> ObjectEntered;

        public event EventHandler<object> ObjectLeft;

        public virtual void Update(GameTime gameTime)
        {
            foreach (var item in SubItems.OfType<IUpdate>().ToArray()) 
            {
                item.Update(gameTime);
            }
        }

        public virtual void AcceptMessageBase(Message message)
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
                tileSide.AcceptMessage(message);
            }
        }

        public abstract IEnumerable<ITileSide> Sides { get; }
        public IRenderer Renderer { get; set; }
    }


 

}