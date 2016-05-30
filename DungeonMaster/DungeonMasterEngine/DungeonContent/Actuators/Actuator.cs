using DungeonMasterEngine.DungeonContent.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class Actuator : Item
    {
        public string DebugString { get; }

        public Actuator(Vector3 position, string s) 
        {
            DebugString = s;
        }

        protected Actuator() { }

        public override string ToString() => DebugString;


    }
}