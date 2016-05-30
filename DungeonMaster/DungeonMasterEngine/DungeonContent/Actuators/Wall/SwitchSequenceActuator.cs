using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class SwitchSequenceActuator : RemoteActuator
    {
        private readonly IEnumerable<Tile> targetTiles;
        private readonly IEnumerable<ActionStateX> actions;
        private bool Pressed = false;

        #region 
        private Texture2D upTexture;

        public Texture2D UpTexture
        {
            get { return upTexture; }
            set
            {
                upTexture = value;
                UpdateTextures();
            }
        }

        private void UpdateTextures()
        {
            ((CubeGraphic) Graphics).Texture = Activated ? downTexture : upTexture;
        }

        private Texture2D downTexture;

        public Texture2D DownTexture
        {
            get { return downTexture; }
            set
            {
                downTexture = value;
                UpdateTextures();
            }
        }
        #endregion


        public SwitchSequenceActuator(Vector3 position, IEnumerable<Tile> targetTiles, IEnumerable<ActionStateX> actions) : base(position)
        {
            this.targetTiles = targetTiles.ToArray();
            this.actions = actions.ToArray();
        }

        protected async void SendMessageAsync(bool activated)
        {
            await Task.WhenAll(targetTiles.Zip(actions, (x,y) => SendOneMessageAsync(x,y,activated)));
        }

        public override IGrabableItem ExchangeItems(IGrabableItem item)
        {
            Pressed ^= true;
            UpdateTextures();

            SendMessageAsync(Pressed);

            return base.ExchangeItems(item);
        }
    }
}