using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class HolderButtonActuator : SimpleRemoteActuator
    {
        private readonly List<IGrabableItem> items;
        public Texture2D Texture
        {
            get { return ((CubeGraphic) Graphics).Texture; }
            set { ((CubeGraphic) Graphics).Texture = value; }
        }


        public HolderButtonActuator(Vector3 position, Tile targetTile, IEnumerable<IGrabableItem> items, ActionStateX action) : base(targetTile, action, position)
        {
            this.items = new List<IGrabableItem>(items);
            Activated = items.Any();
        }

        private void ObjectEntered()
        {
            if (!Activated)
            {
                if (items.Any())
                {
                    Activated = true;
                    SendMessageAsync(Activated);
                }
            }
        }


        private void ObjectLeft()
        {
            if (Activated)
            {
                if (!items.Any()) //hold message
                {
                    Activated = false;
                    SendMessageAsync(Activated);
                }
            }
        }

        public override IGrabableItem ExchangeItems(IGrabableItem item)
        {
            if (item == null)
            {
                var storedItem = items.LastOrDefault();
                if (storedItem != null)
                {
                    items.RemoveAt(items.Count - 1);
                    ObjectLeft();
                }
                return storedItem;
            }
            else
            {
                items.Add(item);
                ObjectEntered();
                return null;
            }
        }
    }
}