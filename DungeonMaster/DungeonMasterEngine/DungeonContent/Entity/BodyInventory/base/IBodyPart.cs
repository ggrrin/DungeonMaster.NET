using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base
{
    public interface IBodyPart : IInventory
    {
        IEnumerable<IEntityPropertyEffect> Effects { get; }

        int Armor { get; }
        int SharpResistance { get; }
        bool IsWound { get; set; }
        float HitProbability { get; }
        float DamageFactor { get; }
    }
}