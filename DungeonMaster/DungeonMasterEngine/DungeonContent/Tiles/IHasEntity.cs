using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public interface IHasEntity
    {
        IEntity Entity { get; }
    }
}