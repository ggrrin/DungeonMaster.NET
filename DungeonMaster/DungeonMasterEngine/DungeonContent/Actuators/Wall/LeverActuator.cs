using System;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterParser.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class LeverActuator : RemoteActuator
    {
        private bool used = false;

        public bool OnceOnly { get; }

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

        private ActionStateX onAction, offAction;
        public override ActionStateX TargetAction => Activated ? onAction : offAction;

        public LeverActuator(Vector3 position, Tile targetTile, bool onceOnly, ActionStateX onAction, ActionStateX offAction) : base(targetTile, position)
        {
            this.onAction = onAction;
            this.offAction = offAction;
            OnceOnly = onceOnly;
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if (OnceOnly && !used)
                Switch();
            else if (!OnceOnly)
                Switch();

            used = true;
            return base.ExchangeItems(item);
        }

        private void Switch()
        {
            Activated ^= true;
            UpdateTextures();
            SendMessage();
        }
    }
}
