using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class AlcoveActuator : Actuator
    {
        private List<GrabableItem> storage;

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
            this.storage = storage.ToList();
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
                    storage.Add(item);
                    $"Item {item} from hand put to aclove".Dump();
                    return null;
                }
                else//take item from alcove
                {
                    if (storage.Count > 0)
                    {
                        var res = storage[0];
                        storage.RemoveAt(0);
                        $"Item {res} took from alcove to hand".Dump();
                        return res;
                    }
                    else
                        return null;
                }
            }
        }
        private void UpdateTextures()
        {
            ((CubeGraphic)Graphics).Texture = Hidden ? hideOutTexture : alcoveTexture;
        }
    }
}
