using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class KeyHoleActuator : RemoteActuator
    {
        /// <summary>
        /// Lock has not been unlocked yet.
        /// </summary>
        public bool Active { get; private set; } = true;

        public IConstrain Constrain { get; }

        private Texture2D decorationTexture;

        public Texture2D DecorationTexture
        {
            get { return decorationTexture; }
            set { decorationTexture = value; UpdateGraphic(); }
        }

        public bool DestroyItem { get; }

        public KeyHoleActuator(Vector3 position, Tile targetTile, ActionStateX action, IConstrain constrain, bool destroyItem) : base(targetTile, action, position)
        {
            Constrain = constrain;
            DestroyItem = destroyItem;
        }

        private void UpdateGraphic()
        {
            ((CubeGraphic)Graphics).Texture = decorationTexture;
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if (Active && Constrain.IsAcceptable(item))
            {
                //TODO how to disable eating stuffs when job is done ???
                //Active = false;
                SendMessage();
                return DestroyItem ? null : item;
            }
            else
            {
                return item;
            }
        }


        public override string ToString()
        {
            return $"Click on item {GetType().Name} with constrain {Constrain}";
        }
    }
}
