using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterEngine.Items
{
    public abstract class GrabableItem : Item
    {
        public int Identifer { get; set; }


        public GrabableItem(Vector3 position) : base(position)
        {

        }


    }
}
