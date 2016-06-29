using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base
{
    public class BodyPart : Inventory, IBodyPart
    {
        public BodyPart(IStorageType storageType, float hitProbability, float damageFactor) : base(storageType)
        {
            HitProbability = hitProbability;
            DamageFactor = damageFactor;
        }

        public IEnumerable<IEntityPropertyEffect> Effects => null;

        public int Armor => 0;

        public int SharpResistance => 0;

        public bool IsWound { get; set; }

        public float HitProbability { get; }

        public float DamageFactor { get; }

        public override string ToString() => Type.GetType().Name;
    }
}