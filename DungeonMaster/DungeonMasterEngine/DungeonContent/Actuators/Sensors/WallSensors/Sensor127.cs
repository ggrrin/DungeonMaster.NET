using System;
using DungeonMasterEngine.DungeonContent.Actuators.Renderers;
using DungeonMasterEngine.DungeonContent.Actuators.Sensors.Initializers;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.GameConsoleContent;

namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.WallSensors
{
    public class Sensor127 : WallSensor<ChampionDecoration>
    {
        private Champion champion;

        public Champion Champion
        {
            get { return champion; }
            set
            {
                if (Disabled)
                    return;

                champion = value;
                Disabled = champion == null;
                if (Disabled)
                {
                    Graphics.ShowChampion = false;
                }
            }
        }

        public bool Empty => Champion == null;

        public Sensor127(ChampionSensorInitializer initializer) : base(initializer)
        {
            Champion = initializer.Champion;
        }

        public override bool TryTrigger(ILeader theron, WallActuator actuator, bool isLast)
        {
            if (!Empty)
            {
                GameConsole.Instance?.RunCommand(new ChampionCommand { Actuator = this });
                return true;
            }

            return base.TryTrigger(theron, actuator, isLast);
        }

        protected override bool TryInteract(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            throw new NotImplementedException();
        }


    }
}