using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class PitTileRenderer : Renderer
    {

        private Matrix translation;
        public Pit Tile { get; }

        public PitTileRenderer(Pit tile)
        {
            Tile = tile;
        }

        public override Matrix GetCurrentTransformation(ref Matrix parentTransformation) => translation * parentTransformation;

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var finalTransformation = GetCurrentTransformation(ref currentTransformation);
            foreach (var side in Tile.WallSides)
            {
                var renderer = side.Renderer;
                renderer.Highlighted = Highlighted;
                renderer.Render(ref finalTransformation, effect, parameter);
            }


            foreach (var subItem in Tile.Drawables.Concat(Tile.SubItems.OfType<IRenderable>()))
                subItem.Renderer?.Render(ref currentTransformation, effect, parameter);

            if (Tile.IsOpen)
            {
                if (Tile.Invisible)
                    RenderFloor(ref finalTransformation, effect, parameter);//render
            }
            else
            {
                RenderFloor(ref finalTransformation, effect, parameter);//render
            }

            return finalTransformation;
        }

        private void RenderFloor(ref Matrix finalTransformation, BasicEffect effect, object parameter)
        {
            if (Tile.Imaginary)
                Tile.FloorSide.Renderer.Render(ref finalTransformation, effect, parameter);//imaginary render
            else
                Tile.FloorSide.Renderer.Render(ref finalTransformation, effect, parameter);//imaginary render
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            var ray = (Ray)leader.Interactor;
            var distance = ray.Intersects(new BoundingBox(Tile.Position, Tile.Position + new Vector3(1f)));

            var finalMatrix = GetCurrentTransformation(ref currentTransformation);
            bool res = false;
            if (distance != null)
            {
                Tile.Renderer.Highlight(500);

                foreach (var tileSide in Tile.WallSides)
                {
                    if (tileSide.Renderer?.Interact(leader, ref finalMatrix, param) ?? false)
                        res = true;
                }
            }

            if (!Tile.IsOpen)
                Tile.FloorSide.Renderer.Interact(leader, ref finalMatrix, param);

            return res;
        }

        public override void Initialize()
        {
            translation = Matrix.CreateTranslation(Tile.Position);
        }
    }
}