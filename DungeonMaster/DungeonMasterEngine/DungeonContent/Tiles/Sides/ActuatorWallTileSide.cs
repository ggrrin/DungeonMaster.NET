using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class ActuatorWallTileSide : TileSide
    {
        public IActuatorX Actuator { get; }

        public ActuatorWallTileSide(IActuatorX actuator, MapDirection direction) : base(direction, false)
        {
            Actuator = actuator;
        }
    }

}