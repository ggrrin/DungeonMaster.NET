namespace DungeonMasterEngine.DungeonContent.Actuators.FloorSensors
{
    public class DirectionIntializer : SensorInitializer<IActuatorX>
    {
        public MapDirection? Direction { get; set; }
    }
}