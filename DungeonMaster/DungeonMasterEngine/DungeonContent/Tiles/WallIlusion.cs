using System;
using System.Linq;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class WallIlusion : WallIlusion<Message>
    {
        public WallIlusion(WallIlusionInitializer initalizer) : base(initalizer) { }
    }

    public class WallIlusion<TMessage> : FloorTile<TMessage> where TMessage : Message
    {
        public bool IsImaginary { get; private set; }
        public bool IsOpen { get; private set; }
        public bool RandomDecoration { get; private set; }

        public override bool IsAccessible => IsImaginary || IsOpen;

        public WallIlusion(WallIlusionInitializer initalizer) : base(initalizer)
        {
            initalizer.Initializing += Initialize;
        }

        private void Initialize(WallIlusionInitializer initializer)
        {
            IsImaginary = initializer.Imaginary;
            IsOpen = initializer.Open;
            RandomDecoration = initializer.RandomDecoration;

            initializer.Initializing -= Initialize;
        }

        public override void ActivateTileContent()
        {
            base.ActivateTileContent();
            IsOpen = true;
        }

        public override void DeactivateTileContent()
        {
            base.DeactivateTileContent();
            IsOpen = false;
        }
    }

    public class WallIllusionRenderer : TileRenderer<WallIlusion>
    {

        public WallIllusionRenderer(WallIlusion tile) : base(tile)
        {

        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            Matrix finalTransformation;
            if (Tile.IsOpen)
            {
                finalTransformation = GetCurrentTransformation(ref currentTransformation);
                foreach (var side in Tile.Sides.Where(x => !(x.Renderer is WallIllusionTileSideRenderer)))
                {
                    var renderer = side.Renderer;
                    renderer.Highlighted = Highlighted;
                    renderer.Render(ref finalTransformation, effect, parameter);
                }
            }
            else
            {
                finalTransformation = base.Render(ref currentTransformation, effect, parameter);
            }

            return finalTransformation;
        }
    }


    public class WallIllusionTileSideRenderer : TileWallSideRenderer<TileSide>
    {
        private readonly Matrix outerWallTransformation = Matrix.CreateTranslation(Vector3.UnitZ * 1.001f) * Matrix.CreateRotationY(MathHelper.Pi);

        public WallIllusionTileSideRenderer(TileSide tileSide, Texture2D wallTexture, Texture2D decorationTexture) : base(tileSide, wallTexture, decorationTexture)
        {


        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var baseTransformation = base.Render(ref currentTransformation, effect, parameter);

            var finalTransformation = outerWallTransformation * baseTransformation;
            RenderWall(effect, ref finalTransformation);

            if (TileSide.RandomDecoration)
                decorationRenderer.Render(ref finalTransformation, effect, parameter);

            return baseTransformation;
        }
    }


}
