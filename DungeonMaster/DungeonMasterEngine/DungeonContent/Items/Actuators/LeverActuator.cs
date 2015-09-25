using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class LeverActuator : Actuator
    {
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

        public Tile TargetTile { get; set; }

        public LeverActuator(Vector3 position, Tile targetTile , bool onceOnly = false ) : base(position)
        {
            TargetTile = targetTile;
            OnceOnly = onceOnly;
        }

        private bool used = false;

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if (OnceOnly && !used)
                Switch();
            else if(!OnceOnly)
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
