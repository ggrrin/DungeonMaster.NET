using DungeonMasterEngine.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class AlcoveActuator : Actuator
    {
        private List<GrabableItem> Storage;

        public bool Hidden { get; set; }


        private Texture2D alcoveTexture;

        public Texture2D AlcoveTexture
        {
            get { return alcoveTexture; }
            set
            {
                alcoveTexture = value;
                UpdateTextures();
            }
        }


        private Texture2D hideOutTexture;

        public Texture2D HideoutTexture
        {
            get { return hideOutTexture; }
            set
            {
                hideOutTexture = value;
                UpdateTextures();
            }
        }


        public AlcoveActuator(Vector3 position, IEnumerable<GrabableItem> storage) : base(position)
        {
            Storage = new List<GrabableItem>(storage);
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if (Hidden)
            {
                Hidden = false;
                UpdateTextures();
                return item;
            }
            else
            {
                if (item != null) //put it to alcove
                {
                    Storage.Add(item);
                    return null;
                }
                else//take item from alcove
                {
                    if (Storage.Count > 0)
                    {
                        var res = Storage[0];
                        Storage.RemoveAt(0);
                        return res;
                    }
                    else
                        return null;
                }
            }
        }
        private void UpdateTextures()
        {
            if(Hidden)
            {
                ((CubeGraphic)Graphics).Texture = hideOutTexture;
            }
            else
            {
                ((CubeGraphic)Graphics).Texture = alcoveTexture;
            }
        }
    }
}
