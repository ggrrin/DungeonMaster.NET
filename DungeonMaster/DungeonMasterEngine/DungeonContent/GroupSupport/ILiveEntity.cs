using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface ILiveEntity : IEntity, IMovable<ISpaceRouteElement>
    {
        IRelationManager RelationManager { get; }
        IBody Body { get; }
        bool Activated { get; set; }

        ISkill GetSkill(ISkillFactory skillType);
    }
}