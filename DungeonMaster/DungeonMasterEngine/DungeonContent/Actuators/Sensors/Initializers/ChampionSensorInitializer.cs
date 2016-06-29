using DungeonMasterEngine.DungeonContent.Actuators.Renderers;
using DungeonMasterEngine.DungeonContent.Entity;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers
{
    public class ChampionSensorInitializer : SensorInitializer<ChampionDecoration>
    {
        public Champion Champion { get; set; }
        public Point GridPosition { get; set; }

    }
}