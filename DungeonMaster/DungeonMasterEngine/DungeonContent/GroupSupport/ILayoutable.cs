using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface ILayoutable : IMovable<ISpaceRouteElement> 
    {
        IGroupLayout GroupLayout { get; }
        IRelationManager RelationManager { get; }
    }
}