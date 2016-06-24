using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.WallSensors
{
    public abstract class Sensor<TActuatorX> : WallSensor, IMessageAcceptor<Message> where TActuatorX : IActuatorX
    {

        public TActuatorX Graphics { get; }
        public override IActuatorX GraphicsBase => Graphics;

        public Sensor(SensorInitializer<TActuatorX> initializer) : base(initializer)
        {
            
            Graphics = initializer.Graphics;
        }
    }
}