using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public abstract class GrabableItem : Item
    {
        public int Identifer { get; set; }

        public int TableIndex { get; set; }
        public string Name { get; set; }


        public GrabableItem(Vector3 position) : base(position)
        {

        }


        public override string ToString()
        {
            return $"{GetType().Name} : {Name} | {Identifer}";
        }
    }
}
