using System;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.GameConsoleContent;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class ChampionSensorInitializer : SensorInitializer<ChampionDecoration>
    {
        public Champion Champion { get; set; }
        public Point GridPosition { get; set; }

    }

    public class Sensor127 : Sensor<ChampionDecoration>
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

        protected override bool Interact(ILeader theron, WallActuator actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            throw new NotImplementedException();
        }


    }

    public class ChampionDecoration : DecorationItem
    {
        public bool ShowChampion { get; set; }

        public ChampionDecoration(bool showChampion)
        {
            ShowChampion = showChampion;
        }
    }
}