
using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Items
{
    public class ChampoinActuator : Actuator
    {
        public Champoin Champoin { get; private set; }

        public bool Empty => Champoin == null;

        public ChampoinActuator(Vector3 position, Champoin champoin) :  base(position)
        {
            Champoin = champoin;
        }

        public void RemoveChampoin()
            => Champoin = null;

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if(!Empty)
            {
                GameConsole.Instance?.RunCommand(new ChampoinCommand { Actuator = this });
            }
            return base.ExchangeItems(item);
        }

        
    }
}