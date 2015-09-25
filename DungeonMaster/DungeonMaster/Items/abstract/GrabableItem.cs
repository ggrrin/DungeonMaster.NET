using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterParser
{
    public abstract class GrabableItem : SuperItem
    {
        public int ItemTypeIndex { get; set; }
    }
}
