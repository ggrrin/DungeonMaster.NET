using System;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class TileSide : IRenderable, IMessageAcceptor<Message>
    {
        public MapDirection Face { get; }
        public bool RandomDecoration { get; }

        public TileSide(MapDirection face, bool randomDecoration)
        {
            Face = face;
            RandomDecoration = randomDecoration;
        }

        public Renderer Renderer { get; set; }

        public virtual void AcceptMessage(Message message) { }

        
    }

    public class TextTileSide : TileSide
    {
        public bool TextVisible { get; private set; }
        public string Text { get; }

        public TextTileSide(MapDirection face, bool textVisible, string text) : base(face, false)
        {
            TextVisible = textVisible;
            Text = text;
        }

        public override void AcceptMessage(Message message)
        {
            switch (message.Action)
            {
                case MessageAction.Set:
                    TextVisible = true;
                    break;
                case MessageAction.Clear:
                    TextVisible = false;
                    break;
                case MessageAction.Toggle:
                    TextVisible ^= true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class TextTileSideRenderer : TileWallSideRenderer<TextTileSide>
    {
        private readonly TextureRenderer textRenderer;

        public TextTileSideRenderer(TextTileSide tileSide, Texture2D wallTexture) : base(tileSide, wallTexture, null)
        {
            textRenderer = new TextureRenderer(Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(new Vector3(0, 0, -0.499f)),
                ResourceProvider.Instance.DrawRenderTarget(TileSide.Text.Replace('|', '\n'), Color.Transparent, Color.White));
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var finalMatrix = base.Render(ref currentTransformation, effect, parameter);

            if (TileSide.TextVisible)
                textRenderer.Render(ref finalMatrix, effect, parameter);

            return finalMatrix;
        }
    }
}