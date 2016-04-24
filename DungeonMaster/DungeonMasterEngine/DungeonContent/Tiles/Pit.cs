using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class Pit : Floor
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

        public Pit(Vector3 position) : base(position)
        {
            wallGraphic.DrawFaces = CubeFaces.None;
            var pitGraphic = new CubeGraphic { Position = position - Vector3.Up, DrawFaces = CubeFaces.Sides };
            pitGraphic.Resources.Content.Load<Texture2D>("Textures/Wall");

            graphics = new GraphicsCollection(wallGraphic, pitGraphic);
            graphicsProviders.SubProviders.Add(graphics);
            base.ContentActivated = true;
        }

        protected override void UpdateWall()
        {
            base.UpdateWall();

            wallGraphic.DrawFaces = IsOpen ? wallGraphic.DrawFaces & ~CubeFaces.Floor : wallGraphic.DrawFaces | CubeFaces.Floor;
        }
    }
}
