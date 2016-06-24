namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class SensorInitializer<TActuatorX> : SensorInitializerX where TActuatorX : IActuatorX
    {
        public TActuatorX Graphics { get; set; }
    }
}