using System;
using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    abstract class ItemConstrainSensor<TActuatorX> : Sensor<TActuatorX> where TActuatorX : IActuatorX
    {
        public IGrabableItemFactoryBase Data { get; }

        protected ItemConstrainSensor(ItemConstrainSensorInitalizer<TActuatorX> initializer) : base(initializer)
        {
            if(initializer.Data == null)
                throw new ArgumentNullException();
            Data = initializer.Data;
        }

        protected abstract override bool TryInteract(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor);
    }
}