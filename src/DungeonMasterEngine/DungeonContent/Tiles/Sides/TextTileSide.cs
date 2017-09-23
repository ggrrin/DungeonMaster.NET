using System;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Tiles.Sides
{
    public class TextTileSide : TileSide
    {
        public bool TextVisible { get; private set; }
        public string Text { get; }

        public TextTileSide(MapDirection face, bool textVisible, string text) : base(face)
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
}