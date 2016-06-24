using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;

namespace DungeonMasterEngine.DungeonContent.Actuators.Mess
{
    struct APart1
    {
        public uint Unreferenced;//:2;
        public bool OnceOnly;//:1;
        public SensorEffect Effect;//:2; /* Not used for group generators */
        public bool RevertEffect;//:1; /* Not used for group generators */
        public bool Audible;//:1;
        public IGrabableItemFactoryBase Value;//:4; /* Ticks for all sensors except group generators (where Bit 10: 0 fixed number, 1 random number and Bits 9-7: count) and end game (where the value is a delay in seconds) */
        public bool LocalEffect;//:1; /* Not used for group generators */
        public uint OrnamentOrdinal;//:4;
    }
}
