using DungeonMasterEngine.DungeonContent.Items.Actuators;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
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