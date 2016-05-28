using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public abstract class GrabableItem : Item, IGrabableItem
    {
        public IEnumerable<IStorageType> PossibleStorages { get; set; }
        public int Identifer { get; set; }

        public int TableIndex { get; set; }
        public int Weight { get; set; }
        public string Name { get; set; }


        public GrabableItem(Vector3 position) : base(position)
        {
            ((CubeGraphic) Graphics).Texture = ResourceProvider.Instance.DrawRenderTarget(this.GetType().Name, Color.Green, Color.White);

        }


        public override string ToString()
        {
            return $"{GetType().Name} : {Name} | {Identifer}";
        }
    }
}
