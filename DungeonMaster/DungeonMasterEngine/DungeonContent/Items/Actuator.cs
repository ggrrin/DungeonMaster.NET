using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DungeonMasterEngine.Items
{
 


    public class Actuator : Item
    {

        public string s;
        
        public Actuator(Vector3 position, string s) : base(position)
        {
            this.s = s;
        }

        public Actuator(Vector3 position) : base(position)
        { }



        public override string ToString() => s;
    }
}