using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterParser
{
    public enum CarrryLocations
    {
        None = 0
       , Consumable = 1
       , Head = 1 * 2
       , Neck = 2 * 2
       , Torso = 4 * 2
       , Legs = 8 * 2
       , Feet = 16 * 2
       , Quiver1 = 32 * 2
       , Quiver2 = 64 * 2
       , Pouch = 128 * 2
       , Hands = 256 * 2
       , Chest = 512 * 2
       , HandsAndBackpack = 1024 * 2
    }
}
