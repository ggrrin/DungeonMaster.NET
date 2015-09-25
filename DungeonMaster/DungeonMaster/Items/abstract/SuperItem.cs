using DungeonMasterParser.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterParser
{
    public abstract class SuperItem 
    {

        public ObjectID ObjectID { get; set; }
        public ushort NextObjectID { get; set; }

        public TilePosition TilePosition => ObjectID.TilePosition;

        public abstract T GetItem<T>(IItemCreator<T> t);


        /// <summary>
        /// Property used by builder.
        /// </summary>
        public bool Processed { get; set; }
    }
}
