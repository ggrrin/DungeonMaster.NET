using DungeonMasterEngine.DungeonContent.Actuators.Wall;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class LogicTileInitializer : TileInitializer
    {
        public new event Initializer<LogicTileInitializer> Initializing;
        public ActuatorX Actuator { get; }
    }
}