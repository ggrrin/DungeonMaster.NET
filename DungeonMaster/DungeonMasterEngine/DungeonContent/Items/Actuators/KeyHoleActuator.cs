using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class KeyHoleActuator : Actuator
    {
        /// <summary>
        /// Lock has not been unlocked yet.
        /// </summary>
        public bool Active { get; private set; } = true;

        public IConstrain Constrain { get; }
        public Tile TargetTile { get; }


        private Texture2D decorationTexture;

        public Texture2D DecorationTexture
        {
            get { return decorationTexture; }
            set { decorationTexture = value; UpdateGraphic(); }
        }

        private void UpdateGraphic()
        {
            ((CubeGraphic)Graphics).Texture = decorationTexture;
        }

        public KeyHoleActuator(Vector3 position, Tile targetTile, IConstrain constrain) : base(position)
        {
            TargetTile = targetTile;
            Constrain = constrain;
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if (Active && Constrain.IsAcceptable(item))
            {
                Active = false;
                Activate();
                return null;
            }
            else
            {
                return item;
            }
        }

        protected virtual void Activate()
        {
            TargetTile.ActivateTileContent();
        }
    }
}
