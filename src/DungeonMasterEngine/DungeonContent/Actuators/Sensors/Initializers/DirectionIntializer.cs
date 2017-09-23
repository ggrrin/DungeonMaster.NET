namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers
{
    public class DirectionIntializer : SensorInitializer<IActuatorX>
    {
        public MapDirection? Direction { get; set; }
    }
}