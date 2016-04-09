using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    internal class Container : GrabableItem
    {
        public List<GrabableItem> SubItems { get; } 

        public Container(Vector3 position, List<GrabableItem> content) : base(position)
        {
            SubItems = content;
        }
    }
}