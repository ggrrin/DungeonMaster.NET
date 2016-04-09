using DungeonMasterParser.Enums;

namespace DungeonMasterParser.Items.@abstract
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
