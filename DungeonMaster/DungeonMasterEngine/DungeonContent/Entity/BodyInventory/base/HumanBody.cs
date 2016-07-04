using System.Collections.Generic;
using System.Linq;

namespace DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base
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
            new BodyPart(HeadStorageType.Instance, 0.06f, 50),
            new BodyPart(NeckStorageType.Instance, 0.04f, 50),
            new BodyPart(TorsoStorageType.Instance, 0.2f, 50),
            new BodyPart(ActionHandStorageType.Instance, .15f, 50),
            new BodyPart(ReadyHandStorageType.Instance, .15f, 50),
            new BodyPart(LegsStorageType.Instance, 0.2f, 50),
            new BodyPart(FeetsStorageType.Instance, 0.2f, 50),
        };

        public IEnumerable<IInventory> Storages => BodyParts.Concat(externalStorage);

        public IInventory GetStorage(IStorageType type) => Storages.FirstOrDefault(s => s.Type == type);

        public IBodyPart GetBodyStorage(IStorageType storageType) => BodyParts.FirstOrDefault(s => s.Type == storageType);
    }
}