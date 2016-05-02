using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    internal class Scroll : GrabableItem
    {
        public string Text { get; set; }

        public Scroll(Vector3 position, string text) : base(position)
        {
            Text = text;
        }
    }
}