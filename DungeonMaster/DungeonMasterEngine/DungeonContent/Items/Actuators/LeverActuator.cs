using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterParser.Enums;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class LeverActuator : RemoteActuator
    {
        private bool used = false;

        public bool Activated { get; private set; }

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

        public Direction ActionDirection { get; private set; }

        public LeverActuator(Vector3 position, Tile targetTile, bool onceOnly, ActionStateX action, Direction actionDirection) : base(targetTile, action, position)
        {
            OnceOnly = onceOnly;
            ActionDirection = actionDirection;
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
            if (Activated)
                Activate();
            else
                Deactivate();

            TargetTile.ExecuteContentActivator(new LogicTileActivator(TargetAction));
        }

        private void Deactivate()
        {
            TargetTile.DeactivateTileContent();
        }

        private void Activate()
        {
            TargetTile.ActivateTileContent();
        }
    }
}
