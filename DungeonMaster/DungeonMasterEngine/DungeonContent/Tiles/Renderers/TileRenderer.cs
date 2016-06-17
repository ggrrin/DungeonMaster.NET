using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class TileRenderer<TTile> : Renderer where TTile : Tile
    {
        private Matrix translation;
        public TTile Tile { get; }

        public TileRenderer(TTile tile)
        {
            Tile = tile;
        }

        public override Matrix GetCurrentTransformation(ref Matrix parentTransformation) => translation * parentTransformation;

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var finalTransformation = GetCurrentTransformation(ref currentTransformation);
            foreach (var side in Tile.Sides)
            {
                var renderer = side.Renderer;
                renderer.Highlighted = Highlighted;
                renderer.Render(ref finalTransformation, effect, parameter);
            }

            foreach (var subItem in Tile.SubItems)
                (subItem as IRenderable)?.Renderer?.Render(ref currentTransformation, effect, parameter);

            return finalTransformation;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            var ray = (Ray)leader.Interactor;
            var distance = ray.Intersects(new BoundingBox(Tile.Position, Tile.Position + new Vector3(1f)));

            var finalMatrix = GetCurrentTransformation(ref currentTransformation);


            bool res = false;
            if (leader.Location == Tile)
            {
                if (distance != null)
                {
                    Tile.Renderer.Highlight(500);

                    foreach (var tileSide in Tile.Sides)
                    {
                        if (tileSide.Renderer?.Interact(leader, ref finalMatrix, param) ?? false)
                            res = true;
                    }
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