using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class TileRenderer : Renderer
    {
        private Matrix translation;
        public Tile Tile { get; }

        public TileRenderer(Tile tile)
        {
            Tile = tile;
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var finalTransformation = translation * currentTransformation;
            foreach (var side in Tile.Sides)
            {
                var renderer = side.Renderer;
                renderer.Highlighted = Highlighted;
                renderer.Render(ref finalTransformation, effect, parameter);
            }

            return finalTransformation;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            var ray = (Ray)leader.Interactor;
            var distance = ray.Intersects(new BoundingBox(Tile.Position, Tile.Position + new Vector3(1f)));

            bool res = false;
            if (distance != null)
            {
                Tile.Renderer.Highlight(500);

                Matrix finalMatrix = translation * currentTransformation;
                foreach (var tileSide in Tile.Sides)
                {
                    if (tileSide.Renderer?.Interact(leader, ref finalMatrix, param) ?? false)
                        res = true;
                }

            }
            return res;
        }

        public override void Initialize()
        {
            translation = Matrix.CreateTranslation(Tile.Position);
        }
    }
}