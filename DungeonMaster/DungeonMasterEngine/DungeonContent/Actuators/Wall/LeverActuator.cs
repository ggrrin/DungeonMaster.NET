using System;
using System.Threading.Tasks;
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

        public override ActionStateX TargetAction { get; }

        public LeverActuator(Vector3 position, Tile targetTile, bool onceOnly, ActionStateX targetAction) : base(targetTile, position)
        {
            Activated = false;
            TargetAction = targetAction;
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
            SendMessageAsync();
        }

        protected override void PerformMessage(ActionStateX action)
        {
            TargetTile.ExecuteContentActivator(new LogicTileActivator(action));
            Toggle();
        }
    }
}
