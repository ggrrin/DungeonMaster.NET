using System;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public abstract class Item : IItem
    {
        private Tile location;
        private Vector3 position;
        public Graphic Graphics { get; set; }
        public bool Visible { get; set; } = true;
        public bool AcceptMessages { get; set; }
        public MapDirection MapDirection { get; set; }

        public virtual BoundingBox Bounding => new BoundingBox(Position, Position + Graphics.Scale);
        //public sealed override IGraphicProvider GraphicsProvider => Visible ? Graphics : null;

        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                if (Graphics != null)
                    Graphics.Position = value;
            }
        }



        public IRenderer Renderer { get; set; }
        public IInteractor Inter { get; set; }

        public Tile Location
        {
            get
            {
                return location;
            }

            set
            {
                //old
                location?.OnObjectLeft(this);

                location = value;
                
                location?.OnObjectEntered(this);
                OnLocationChanged();
            }
        }

        protected Item() 
        {
            Graphics = new CubeGraphic
            {
                Scale = new Vector3(0.25f),
                DrawFaces = CubeFaces.All ^ CubeFaces.Floor,
                Outter = true
            };
        }

        public virtual IGrabableItem ExchangeItems(IGrabableItem item)
        {
            return item;
        }

        protected virtual void OnLocationChanged()
        {
            Position = location?.Position ?? Vector3.Zero;
        }

        public virtual void Update(GameTime gameTime)
        { }
    }
}
