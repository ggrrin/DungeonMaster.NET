using System.Collections.Generic;
using System.Linq;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
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