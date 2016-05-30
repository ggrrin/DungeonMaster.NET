using System.Collections.Generic;
using System.Linq;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base
{
    public class HumanBody : IBody
    {
        private readonly IEnumerable<IInventory> externalStorage = new IInventory[]
        {
            new Inventory(BackPackStorageType.Instance),
            new Inventory(PouchStorageType.Instance),
            new Inventory(SmallQuiverStorageType.Instance),
            new Inventory(BigQuiverStorageType.Instance),
        };

        public IEnumerable<IBodyPart> BodyParts { get; } = new IBodyPart[]
        {
            new BodyPart(HeadStorageType.Instance, 0.06f, 1.3f),
            new BodyPart(NeckStorageType.Instance, 0.04f, 1.5f),
            new BodyPart(TorsoStorageType.Instance, 0.2f, 0.7f),
            new BodyPart(ActionHandStorageType.Instance, 15f, 0.8f),
            new BodyPart(HandStorageType.Instance, 15f, 0.8f),
            new BodyPart(LegsStorageType.Instance, 0.2f, 0.9f),
            new BodyPart(FeetsStorageType.Instance, 0.2f, 0.9f),
        };

        public IEnumerable<IInventory> Storages => BodyParts.Concat(externalStorage);
        public bool IsWound { get; }

        public IInventory GetStorage(IStorageType type) => Storages.FirstOrDefault(s => s.Type == type);

        public IBodyPart GetBodyStoratge(IStorageType storageType) => BodyParts.FirstOrDefault(s => s.Type == storageType);
    }
}