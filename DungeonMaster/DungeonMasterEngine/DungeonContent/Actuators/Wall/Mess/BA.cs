namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    struct BA
    { /* Regular with remote target */
        public uint Unreferenced;//:4;
        public EarthSides TargetCell;//:2; /* Ignored for squares other than walls */
        public int TargetMapX;//:5;
        public int TargetMapY;//:5;
    }
}