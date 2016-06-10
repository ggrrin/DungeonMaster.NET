using System;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    abstract class ItemConstrainSensor : Sensor
    {
        public IGrabableItemFactoryBase Data { get; }

        protected ItemConstrainSensor(ItemConstrainSensorInitalizer initializer) : base(initializer)
        {
            if(initializer.Data == null)
                throw new ArgumentNullException();
            Data = initializer.Data;
        }

        protected abstract override bool Interact(ILeader theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor);
    }
}