using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public interface IHasEntity
    {
        IEntity Entity { get; }
    }
}