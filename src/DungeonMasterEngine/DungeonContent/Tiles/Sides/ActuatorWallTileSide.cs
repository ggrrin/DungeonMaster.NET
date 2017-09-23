using DungeonMasterEngine.DungeonContent.Actuators;

namespace DungeonMasterEngine.DungeonContent.Tiles.Sides
{
    public class ActuatorWallTileSide : TileSide
    {
        public IActuatorX Actuator { get; }

        public ActuatorWallTileSide(IActuatorX actuator, MapDirection direction) : base(direction)
        {
            Actuator = actuator;
        }
    }

}