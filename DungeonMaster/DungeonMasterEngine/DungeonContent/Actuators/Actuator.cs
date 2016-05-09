using DungeonMasterEngine.DungeonContent.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class Actuator : Item
    {
        public string DebugString { get; }

        public Actuator(Vector3 position, string s) : base(position)
        {
            DebugString = s;
        }

        protected Actuator(Vector3 position) : base(position) { }

        public override string ToString() => DebugString;


    }
}