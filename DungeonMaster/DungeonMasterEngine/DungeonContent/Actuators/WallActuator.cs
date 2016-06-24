using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators.WallSensors;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class WallActuator : ActuatorX
    {
        public List<WallSensor> Sensors { get; } 
 
        public WallActuator(IEnumerable<WallSensor> sensors) 
        {
            if (sensors == null || sensors.Any(x => x == null))
                throw new ArgumentNullException();

            Sensors = new List<WallSensor>(sensors);
        }

        public bool Trigger(ILeader leader)
        {
            bool anyTriggered = false;

            foreach (var sensor in Sensors)
            {
                bool sensorTrigger = sensor.TryTrigger(leader, this, sensor == Sensors.Last());
                anyTriggered = anyTriggered || sensorTrigger;
            }

            F271_xxxx_SENSOR_ProcessRotationEffect(Sensors);
            return anyTriggered;
        }

        public override IEnumerable<SensorX> SensorsEnumeration => Sensors;
    }
}