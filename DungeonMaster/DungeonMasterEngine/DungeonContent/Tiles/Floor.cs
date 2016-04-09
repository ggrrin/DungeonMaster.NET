using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class Floor : Tile
    {
        protected CubeGraphic wallGraphic;

        public Floor(Vector3 position):base(position)
        {
            Position = position;
            wallGraphic = new CubeGraphic { Position = position };
            wallGraphic.Texture = wallGraphic.Resources.Content.Load<Texture2D>("Textures/Wall");

            graphicsProviders.SubProviders.Add(wallGraphic);
        }

        public override INeighbours Neighbours
        {
            get
            {
                return base.Neighbours;
            }

            set
            {
                base.Neighbours = value;
                UpdateWall();
            }
        }

        protected virtual void UpdateWall()
        {
            wallGraphic.DrawFaces = CubeFaces.All; //reset

            if (Neighbours.North != null)
                wallGraphic.DrawFaces ^= CubeFaces.Back;
            if (Neighbours.East != null)
                wallGraphic.DrawFaces ^= CubeFaces.Right;
            if (Neighbours.South != null)
                wallGraphic.DrawFaces ^= CubeFaces.Front;
            if (Neighbours.West != null)
                wallGraphic.DrawFaces ^= CubeFaces.Left;
        }
        public override bool IsAccessible => true;
    }
}
