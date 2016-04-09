using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class Counter : RemoteActuator
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
                        SendMessage();
                }
            }
        }

        public Counter(Tile targetTile, ActionStateX action, int startCount, Vector3 position) : base(targetTile, action, position)
        {
            count = startCount;
        }

        public void Increase() => Count++;

        public void Decrease() => Count--;
    }
}
