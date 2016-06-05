using System;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{

    public class Pit : Pit<Message>
    {
        public Pit(PitInitializer initializer) : base(initializer) {}
    }


    public class PitInitializer : FloorInitializer
    {
        public new event Initializer<PitInitializer> Initializing;
        public bool IsImaginary { get; set; }

        public bool IsVisible { get; set; }
        
        public bool IsOpen { get; set; }
    }

    public class Pit<TMessage> : Floor<TMessage> where TMessage : Message
    {
        private GraphicsCollection graphics;


        public bool IsOpen => ContentActivated;

        public override bool ContentActivated
        {
            get
            {
                return base.ContentActivated;
            }

            protected set
            {
                base.ContentActivated = value;
                UpdateWall();
            }
        }

        public override bool IsAccessible => true;

        public override Vector3 StayPoint => IsOpen ? base.StayPoint + Vector3.Down : base.StayPoint;

        public Pit(PitInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
            //TODO uncomment
            //wallGraphic.DrawFaces = CubeFaces.None;
            //var pitGraphic = new CubeGraphic { Position = position - Vector3.Up, DrawFaces = CubeFaces.Sides };
            //pitGraphic.Resources.Content.Load<Texture2D>("Textures/Wall");

            //graphics = new GraphicsCollection(wallGraphic, pitGraphic);
            //graphicsProviders.SubProviders.Add(graphics);
            //base.ContentActivated = true;
        }

        private void Initialize(PitInitializer initializer)
        {
            //TODO initalize
            initializer.Initializing -= Initialize;
        }

        protected override void UpdateWall()
        {
            base.UpdateWall();

            //TODO uncomment
            //wallGraphic.DrawFaces = IsOpen ? wallGraphic.DrawFaces & ~CubeFaces.Floor : wallGraphic.DrawFaces | CubeFaces.Floor;
        }
    }
}
