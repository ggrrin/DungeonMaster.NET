using DungeonMasterEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Tiles
{
    public class Pit : Floor
    {
        private GraphicsCollection graphics;

        private bool isOpen = true;

        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; UpdateWall(); }
        }

        public Pit(Vector3 position) : base(position)
        {
            wallGraphic.DrawFaces = CubeFaces.None;
            var pitGraphic = new CubeGraphic { Position = position - Vector3.Up, DrawFaces = CubeFaces.Sides };
            pitGraphic.Resources.Content.Load<Texture2D>("Textures/Wall");

            graphics = new GraphicsCollection(base.wallGraphic, pitGraphic);
            graphicsProviders.SubProviders.Add(graphics);
        }

        protected override void UpdateWall()
        {
            base.UpdateWall();

            wallGraphic.DrawFaces = IsOpen ? wallGraphic.DrawFaces & ~CubeFaces.Floor : wallGraphic.DrawFaces | CubeFaces.Floor;
        }

        public override void ActivateTileContent()
        {
            IsOpen = false;
            base.ActivateTileContent();
        }

        public override void DeactivateTileContent()
        {
            base.DeactivateTileContent();
            IsOpen = true;
        }

        public override bool IsAccessible => !IsOpen;
   

    }
}
