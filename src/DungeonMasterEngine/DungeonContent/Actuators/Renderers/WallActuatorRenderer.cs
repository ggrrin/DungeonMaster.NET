using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Renderers
{

    public class WallActuatorRenderer : ActuatorRenderer<WallActuator>
    {


        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            bool decorationInteract = Actuator.SensorsEnumeration.LastOrDefault()?.GraphicsBase.Renderer.Interact(leader, ref currentTransformation, param) ?? false;

            if (decorationInteract)
                Actuator.Trigger(leader);

            return decorationInteract;
        }

        public WallActuatorRenderer(WallActuator actuator) : base(actuator) { }
    }
}