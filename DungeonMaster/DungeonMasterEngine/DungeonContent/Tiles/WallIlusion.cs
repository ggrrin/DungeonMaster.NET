using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Tiles
{
    public class WallIlusion : Floor
    {
        public WallIlusion(Vector3 position) : base(position)
        {
            //todo illusion wall face
            wallGraphic.Texture = wallGraphic.Resources.DefaultTexture;
        }
    }
}
