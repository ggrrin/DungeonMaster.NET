using System;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators.Wall.FloorSensors;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{

    public class WallActuatorRenderer : ActuatorRenderer<WallActuator>
    {


        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            if (Actuator.SensorsEnumeration.LastOrDefault()?.GraphicsBase.Renderer.Interact(leader, ref currentTransformation, param) ?? false)
            {
                return Actuator.Trigger(leader);
            }
            return false;
        }

        public WallActuatorRenderer(WallActuator actuator) : base(actuator) {}
    }

    public class ActuatorRenderer<TActuator> : Renderer where TActuator : ActuatorX
    {
        public TActuator Actuator { get; }

        public ActuatorRenderer(TActuator actuator)
        {
            Actuator = actuator;
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            Actuator.SensorsEnumeration.LastOrDefault()?.GraphicsBase?.Renderer?.Render(ref currentTransformation, effect, parameter);
            return currentTransformation;
        }

        public override Matrix GetCurrentTransformation(ref Matrix parentTransformation) => parentTransformation;

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            return false;
        }
    }
}