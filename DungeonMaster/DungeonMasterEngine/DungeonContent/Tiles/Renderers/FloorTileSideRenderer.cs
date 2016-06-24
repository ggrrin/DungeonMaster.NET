using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class FloorTileSideRenderer<TFloorTileSide> : TileWallSideRenderer<TFloorTileSide> where TFloorTileSide : FloorTileSide
    {
        public FloorTileSideRenderer(TFloorTileSide tileSide, Texture2D wallTexture, Texture2D decorationTexture) : base(tileSide, wallTexture, decorationTexture) { }

        const float size = 0.25f;

        public Vector3 GetPosition(Point p)
        {
            return new Vector3(p.X, p.Y, 0) * size - new Vector3(0, 0, 0.499f);
        }

        public BoundingBox GetBoundingBox(Point p)
        {
            var pos = GetPosition(p);
            return new BoundingBox(pos + new Vector3(-size, size, 0), pos + new Vector3(size, -size, 0.01f));

        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            Matrix finalTransformation = base.Render(ref currentTransformation, effect, parameter);

            int j = 1;
            foreach (var space in TileSide.Spaces)
            {
                Matrix floorStorageTransformation = Matrix.CreateTranslation(GetPosition(space.GridPosition) + new Vector3(0, 0, j * Epsilon)) * finalTransformation;
                foreach (var item in space.Items)
                    item.Renderer.Render(ref floorStorageTransformation, effect, parameter);
            }

            return finalTransformation;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {

            Ray ray = (Ray)leader.Interactor;
            var resultTransformationInverse = Matrix.Invert(GetCurrentTransformation(ref currentTransformation));

            foreach (var space in TileSide.Spaces)
            {
                var p = GetBoundingBox(space.GridPosition);
                var transformedRay = ray.Transform(ref resultTransformationInverse);
                if (transformedRay.Intersects(p) != null)
                {
                    space.Trigger(leader);
                    break;
                }
            }

            return false;
        }
    }
}