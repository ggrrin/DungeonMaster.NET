using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public interface ILiveEntity : IEntity, IMovable<ISpaceRouteElement>
    {
        IGroupLayout GroupLayout { get; }

        IRelationManager RelationManager { get; }
        IBody Body { get; }
        bool Activated { get; set; }

        ISkill GetSkill(ISkillFactory skillType);
    }
}