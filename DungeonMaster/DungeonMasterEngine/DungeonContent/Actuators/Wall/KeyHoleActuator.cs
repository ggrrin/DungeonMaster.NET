using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class KeyHoleActuator : RemoteActuator
    {
        private readonly IEnumerable<Tile> targetTile;
        private readonly IEnumerable<ActionStateX> action;
        public IConstrain Constrain { get; }

        private Texture2D decorationTexture;

        public Texture2D DecorationTexture
        {
            get { return decorationTexture; }
            set { decorationTexture = value; UpdateGraphic(); }
        }

        public bool DestroyItem { get; }

        public KeyHoleActuator(Vector3 position, IEnumerable<Tile> targetTile, IEnumerable<ActionStateX> action, IConstrain constrain, bool destroyItem) : base(position)
        {
            this.targetTile = targetTile;
            this.action = action;
            Constrain = constrain;
            DestroyItem = destroyItem;
        }

        private void UpdateGraphic()
        {
            ((CubeGraphic)Graphics).Texture = decorationTexture;
        }

        public override IGrabableItem ExchangeItems(IGrabableItem item)
        {
            if (Activated && Constrain.IsAcceptable(item))
            {
                //TODO how to disable eating stuffs when job is done ???
                //Active = false;
                SendMessageAsync(activated: true);
                return DestroyItem ? null : item;
            }
            else
            {
                return item;
            }
        }

        private async void SendMessageAsync(bool activated )
        {
            await Task.WhenAll(targetTile.Zip(action, (x,y) => SendOneMessageAsync(x,y,activated)));

        }

        public override string ToString()
        {
            return $"Click on item {GetType().Name} with constrain {Constrain}";
        }
    }
}
