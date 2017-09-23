using DungeonMasterParser.Enums;

namespace DungeonMasterParser.Items
{
    public abstract class ItemData 
    {

        public ObjectID ObjectID { get; set; }
        public ushort NextObjectID { get; set; }

        public TilePosition TilePosition => ObjectID.TilePosition;

        public abstract T CreateItem<T>(IItemCreator<T> t);


        /// <summary>
        /// Property used by builder.
        /// </summary>
        public bool Processed { get; set; }
    }
}
