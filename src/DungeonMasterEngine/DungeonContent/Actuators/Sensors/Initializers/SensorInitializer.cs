namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers
{
    public class SensorInitializer<TActuatorX> : SensorInitializerX where TActuatorX : IActuatorX
    {
        public TActuatorX Graphics { get; set; }
    }
}