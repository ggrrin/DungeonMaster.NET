using System;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class ActuatorXRenderer : Renderer
    {
        public ActuatorX Actuator { get; }

        public ActuatorXRenderer(ActuatorX actuator)
        {
            Actuator = actuator;
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            Actuator.Sensors.LastOrDefault()?.Graphics.Renderer.Render(ref currentTransformation, effect, parameter);
            return currentTransformation;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            if (Actuator.Sensors.LastOrDefault()?.Graphics.Renderer.Interact(leader, ref currentTransformation, param) ?? false)
            {
                return Actuator.F275_aszz_SENSOR_IsTriggeredByClickOnWall(leader);
            }
            return false;
        }
    }
}