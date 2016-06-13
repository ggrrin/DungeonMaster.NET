using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class SensorInitializer<TActuatorX> : SensorInitializerX where TActuatorX : IActuatorX
    {
        public TActuatorX Graphics { get; set; }
    }

    public class SensorInitializerX 
    {
        public SensorEffect Effect { get; set; }
        //local target
        public bool ExperienceGain { get; set; }
        public bool Rotate { get; set; }

        //remote target
        public MapDirection Specifer { get; set; }
        public Tile TargetTile { get; set; }

        public int TimeDelay { get; set; }
        public bool LocalEffect { get; set; }
        public bool RevertEffect { get; set; }
        public bool OnceOnly { get; set; }
        public bool Audible { get; set; }
    }
}