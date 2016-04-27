using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class HolderButtonActuator : RemoteActuator
    {
        private readonly List<GrabableItem> items;
        public Texture2D Texture
        {
            get { return ((CubeGraphic) Graphics).Texture; }
            set { ((CubeGraphic) Graphics).Texture = value; }
        }

        public override ActionStateX TargetAction { get; }

        public HolderButtonActuator(Vector3 position, Tile targetTile, IEnumerable<GrabableItem> items, ActionStateX action) : base(targetTile, position)
        {
            TargetAction = action;
            this.items = new List<GrabableItem>(items);
            Activated = items.Any();
        }

        private void ObjectEntered()
        {
            if (!Activated)
            {
                if (items.Any())
                {
                    Activated = true;
                    SendMessageAsync();
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
                    SendMessageAsync();
                }
            }
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
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