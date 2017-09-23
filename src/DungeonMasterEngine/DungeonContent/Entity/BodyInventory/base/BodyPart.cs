using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base
{
    public class BodyPart : Inventory, IBodyPart
    {
        public BodyPart(IStorageType storageType, float hitProbability, int injuryMultipler) : base(storageType)
        {
            HitProbability = hitProbability;
            InjuryMultipler = injuryMultipler;
        }

        public IEnumerable<IEntityPropertyEffect> Effects => null;

        public int Armor => 0;

        public int SharpResistance => 0;

        public bool IsWounded { get; set; }

        public float HitProbability { get; }

        public int InjuryMultipler { get; }

        public override string ToString() => Type.GetType().Name;
    }
}