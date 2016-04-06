using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterEngine.Items
{
    //TODO: extension objects =>  new object -> new class vs parameter class
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


        public Item(Vector3 position) : base(position)
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

        public event EventHandler LocationChanged;

        public Tile Location
        {
            get
            {
                return location;
            }

            set
            {
                var oldLocation = location;
                if (oldLocation != null)
                {
                    oldLocation.OnObjectLeft(this);
                    oldLocation.SubItems.Remove(this);
                    Position += value.Position - oldLocation.Position;
                }
                else
                {
                    Position = value.Position;
                }

                location = value;
                location.SubItems.Add(this);
                location.OnObjectEntered(this);
                 
            }
        }
    }
}
