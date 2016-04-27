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
        public override ActionStateX TargetAction { get; }
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


        public TimerSwitchActuator(Vector3 position, Tile targetTile, ActionStateX firstAction, ActionStateX secondAction) : base(targetTile, position)
        {
            TargetAction = firstAction;
            SecondAction = secondAction;
        }

        protected override async void SendMessageAsync()
        {
            var t1 = SendOneMessageAsync(TargetAction);
            var t2 = SendOneMessageAsync(SecondAction);
            await Task.WhenAll(t1, t2);
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            Pressed ^= true;
            UpdateTextures();

            SendMessageAsync();

            return base.ExchangeItems(item);
        }
    }
}