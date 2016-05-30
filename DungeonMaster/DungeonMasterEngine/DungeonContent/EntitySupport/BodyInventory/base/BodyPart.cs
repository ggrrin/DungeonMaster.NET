using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base
{
    public class BodyPart : IBodyPart
    {
        private readonly IInventory bodyPartImplementation;
        public IStorageType Type => bodyPartImplementation.Type;

        public IReadOnlyList<IGrabableItem> Storage => bodyPartImplementation.Storage;

        public IGrabableItem TakeItemFrom(int index) => bodyPartImplementation.TakeItemFrom(index);

        public bool AddItemTo(IGrabableItem item, int index) => bodyPartImplementation.AddItemTo(item, index);

        public bool AddItem(IGrabableItem item) => bodyPartImplementation.AddItem(item);

        public IEnumerable<IGrabableItem> AddRange(IEnumerable<IGrabableItem> items) => bodyPartImplementation.AddRange(items);

        public BodyPart(IStorageType storageType, float hitProbability, float damageFactor)
        {
            bodyPartImplementation = new Inventory(storageType);
            HitProbability = hitProbability;
            DamageFactor = damageFactor;

        }

        public IEnumerable<IEntityPropertyEffect> Effects => null;

        public int Armor => 0;

        public int SharpResistance => 0;

        public bool IsWound { get; set; }

        public float HitProbability { get; }

        public float DamageFactor { get; }
    }
}