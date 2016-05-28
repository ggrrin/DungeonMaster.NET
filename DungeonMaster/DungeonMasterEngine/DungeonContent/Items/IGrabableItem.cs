using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public interface IGrabableItem : IItem
    {
        IEnumerable<IStorageType> PossibleStorages { get; set; }
        int Identifer { get; set; }
        string Name { get; set; }
        int TableIndex { get; set; }
        int Weight { get; set; }
    }
}