using DungeonMasterEngine.GameConsoleContent;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
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
                GameConsole.Instance?.RunCommand(new ChampionCommand { Actuator = this });
            }
            return base.ExchangeItems(item);
        }

        
    }
}