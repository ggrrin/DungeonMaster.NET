using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
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