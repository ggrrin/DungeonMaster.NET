using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class TimerSwitchActuator : RemoteActuator
    {
        private bool Pressed = false;

        public Tile TargetTile { get; }
        public ActionStateX FirstAction { get; }
        public ActionStateX SecondAction { get; }


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


        public TimerSwitchActuator(Vector3 position, Tile targetTile, ActionStateX firstAction, ActionStateX secondAction) : base(position)
        {
            TargetTile = targetTile;
            FirstAction = firstAction;
            SecondAction = secondAction;
        }


        protected async void SendMessageAsync(bool activated)
        {
            var t1 = SendOneMessageAsync(TargetTile, FirstAction, activated);
            var t2 = SendOneMessageAsync(TargetTile, SecondAction, activated);
            await Task.WhenAll(t1, t2);
        }

        public override IGrabableItem ExchangeItems(IGrabableItem item)
        {
            Pressed ^= true;
            UpdateTextures();

            SendMessageAsync(activated: true);

            return base.ExchangeItems(item);
        }
    }
}