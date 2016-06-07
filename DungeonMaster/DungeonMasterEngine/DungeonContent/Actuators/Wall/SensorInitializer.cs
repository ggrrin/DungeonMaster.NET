using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class StorageSensorInitializer : ItemConstrainSensorInitalizer 
    {
        public IGrabableItem StoredItem { get; set; }
    }

    public class ItemConstrainSensorInitalizer : SensorInitializer
    {
        public IGrabableItemFactoryBase Data { get; set; }
    }

    public class SensorInitializer
    {
        public SensorEffect Effect { get; set; }
        //local target
        public bool ExperienceGain { get; set; }
        public bool Rotate { get; set; }

        //remote target
        public EarthSides Specifer { get; set; }
        public Tile TargetTile { get; set; }

        public int TimeDelay { get; set; }
        public bool LocalEffect { get; set; }
        public bool RevertEffect { get; set; }
        public bool OnceOnly { get; set; }
        public bool Audible { get; set; }
        public IActuatorX Graphics { get; set; }


    }
}