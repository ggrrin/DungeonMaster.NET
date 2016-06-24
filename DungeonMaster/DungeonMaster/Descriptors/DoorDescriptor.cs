using DungeonMasterParser.Enums;

namespace DungeonMasterParser.Descriptors
{
    public class DoorDescriptor
    {
        public DoorType Type { get; set; }
        public bool Animated { get; set; }
        public bool ItemsPassThrough{ get; set; }
        public bool CreatureSeeThrough { get; set; }
        public int Resistance { get; set; }
    }
}