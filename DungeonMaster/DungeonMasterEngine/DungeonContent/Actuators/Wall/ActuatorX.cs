using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class ActuatorX : IActuatorX
    {
        public Renderer Renderer { get; set; }

        public ActuatorX(IEnumerable<Sensor> sensors)
        {
            if(sensors == null || sensors.Any(x => x== null))
                throw new ArgumentNullException();

            Sensors = new List<Sensor>(sensors);
        }

        public List<Sensor> Sensors { get; }
        public bool Rotate { get; set; }

        public bool Trigger(ILeader leader)
        {
            bool anyTriggered = false;

            foreach (var sensor in Sensors)
                anyTriggered = anyTriggered || sensor.TryTrigger(leader, this, sensor == Sensors.Last());

            F271_xxxx_SENSOR_ProcessRotationEffect();
            return anyTriggered;
        }

        void F271_xxxx_SENSOR_ProcessRotationEffect()
        {
            if (!Rotate)
                return;

            var first = Sensors.FirstOrDefault();
            if (first != null)
            {
                Sensors.RemoveAt(0);
                Sensors.Add(first);
            }

            Rotate = false;
        }

        public void AcceptMessage(Message message)
        {
            foreach (var sensor in Sensors)
            {
                sensor.AcceptMessage(message);
            }
        }

        public void Interact(ILeader leader, ref Matrix matrix, object param)
        {
            Trigger(leader);
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}