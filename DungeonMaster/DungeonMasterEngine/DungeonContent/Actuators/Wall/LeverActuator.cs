using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class LeverActuator : RemoteActuator
    {
        private bool used = false;

        public bool OnceOnly { get; }

        public Tile TargetTile { get; }

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
            ((CubeGraphic)Graphics).Texture = Activated ? downTexture : upTexture;
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

        public ActionStateX TargetAction { get; }

        public LeverActuator(Vector3 position, Tile targetTile, bool onceOnly, ActionStateX targetAction) : base(position)
        {
            Activated = false;
            TargetAction = targetAction;
            TargetTile = targetTile;
            OnceOnly = onceOnly;
        }

        public override IGrabableItem ExchangeItems(IGrabableItem item)
        {
            if (OnceOnly && !used)
                Switch();
            else if (!OnceOnly)
                Switch();

            used = true;
            return base.ExchangeItems(item);
        }

        private async void Switch()
        {
            Activated ^= true;
            UpdateTextures();
            await SendOneMessageAsync(TargetTile, TargetAction, Activated);
        }

        protected override void PerformMessage(Tile targetTile, ActionStateX action, bool activated)
        {
            targetTile.ExecuteContentActivator(new LogicTileActivator(action));
            Toggle(targetTile);
        }

    }
}
