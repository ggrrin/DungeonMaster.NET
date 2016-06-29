using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base
{
    public interface IBody
    {
        IEnumerable<IBodyPart> BodyParts { get; }

        IEnumerable<IInventory> Storages { get; }

        IInventory GetStorage(IStorageType type);

        IBodyPart GetBodyStoratge(IStorageType storageType);
    }
}