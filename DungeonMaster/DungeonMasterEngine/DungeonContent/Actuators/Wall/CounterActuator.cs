using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class CounterActuator : SimpleRemoteActuator
    {
        private int count;
        public int Count
        {
            get { return count; }
            private set
            {
                if (count != 0)
                {
                    count = value;
                    if (Count == 0)
                        SendMessageAsync(activated: true);
                }
            }
        }


        public CounterActuator(Tile targetTile, ActionStateX action, int startCount, Vector3 position) : base(targetTile,action, position)
        {
            count = startCount;
        }

        public void Increase() => Count++;

        public void Decrease() => Count--;
    }
}
