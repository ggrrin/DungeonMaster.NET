using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public abstract class ActuatorX : IActuatorX
    {
        public IRenderer Renderer { get; set; }

        //public ActuatorX(IEnumerable<SensorX> sensors)
        //{
        //    if(sensors == null || sensors.Any(x => x== null))
        //        throw new ArgumentNullException();

        //    Sensors = new List<SensorX>(sensors);
        //}

        public abstract IEnumerable<SensorX> SensorsEnumeration { get; }

        public bool Rotate { get; set; }

        //F271_xxxx_SENSOR_ProcessRotationEffect
        protected void ProcessRotationEffect<T>(IList<T> sensors)
        {
            if (!Rotate)
                return;

            T first = sensors.FirstOrDefault();
            if (first != null)
            {
                sensors.RemoveAt(0);
                sensors.Add(first);
            }

            Rotate = false;
        }


        public virtual void AcceptMessage(Message message)
        {
            foreach (var sensor in SensorsEnumeration)
            {
                sensor.AcceptMessage(message);
            }
        }

    }
}