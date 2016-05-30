using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class ChampoinActuator : Actuator
    {
        public Champion Champion { get; private set; }

        public bool Empty => Champion == null;

        public ChampoinActuator(Vector3 position, Champion champion)   
        {
            Champion = champion;
        }

        public void RemoveChampoin() => Champion = null;

        public override IGrabableItem ExchangeItems(IGrabableItem item)
        {
            if(!Empty)
            {
                GameConsole.Instance?.RunCommand(new ChampionCommand { Actuator = this });
            }
            return base.ExchangeItems(item);
        }

        
    }
}