using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators.WallSensors;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class LogicActuator : ActuatorX
    {
        public LogicActuator(IEnumerable<SensorX> sensors)
        {
            this.sensors = sensors.ToArray();
        }

        private readonly SensorX[] sensors;

        public override IEnumerable<SensorX> SensorsEnumeration => sensors;

    }
}