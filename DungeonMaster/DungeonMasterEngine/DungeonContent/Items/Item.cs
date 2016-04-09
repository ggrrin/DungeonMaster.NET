using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public abstract class Item : WorldObject, ILocalizable<Tile>
    {
        public Graphic Graphics { get; set; }

        public bool Visible { get; set; } = true;

        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }

            set
            {
                base.Position = value;
                if (Graphics != null)
                    Graphics.Position = value;
            }
        }

        protected Item(Vector3 position) : base(position)
        {
            Graphics = new CubeGraphic
            {
                Position = position,
                Scale = new Vector3(0.2f),
                DrawFaces = CubeFaces.All ^ CubeFaces.Floor,
                Outter = true
            };
        }

        public virtual BoundingBox Bounding => new BoundingBox(Position, Position + Graphics.Scale);


        public virtual GrabableItem ExchangeItems(GrabableItem item)
        {
            return item;
        }

        public sealed override IGraphicProvider GraphicsProvider => Visible ? Graphics : null;

        private Tile location;

        public Tile Location
        {
            get
            {
                return location;
            }

            set
            {
                //old
                location?.SubItems.Remove(this);
                location?.OnObjectLeft(this);

                location = value;
                
                //new
                location?.SubItems.Add(this);
                location?.OnObjectEntered(this);

                Position = value?.Position ?? Vector3.Zero;
            }
        }

        public virtual void Update(GameTime gameTime) { }
    }
}
