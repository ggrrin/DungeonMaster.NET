using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base
{
    public interface IBody
    {
        IEnumerable<IBodyPart> BodyParts { get; }

        IEnumerable<IInventory> Storages { get; }
        bool IsWound { get; }

        IInventory GetStorage(IStorageType type);

        IBodyPart GetBodyStoratge(IStorageType storageType);
    }
}