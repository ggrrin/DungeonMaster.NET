using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
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

    public abstract class ActuatorX : IActuatorX
    {
        public Renderer Renderer { get; set; }

        //public ActuatorX(IEnumerable<SensorX> sensors)
        //{
        //    if(sensors == null || sensors.Any(x => x== null))
        //        throw new ArgumentNullException();

        //    Sensors = new List<SensorX>(sensors);
        //}

        public abstract IEnumerable<SensorX> SensorsEnumeration{ get; }

        public bool Rotate { get; set; }


        protected void F271_xxxx_SENSOR_ProcessRotationEffect<T>(IList<T> sensors )
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