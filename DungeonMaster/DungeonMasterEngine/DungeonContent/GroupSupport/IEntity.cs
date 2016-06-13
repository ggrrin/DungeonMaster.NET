using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface ILiveEntity : IEntity, IMovable<ISpaceRouteElement>
    {
        IRelationManager RelationManager { get; }
        IBody Body { get; }

        ISkill GetSkill(ISkillFactory skillType);
    }

    public interface IEntity
    {
        IGroupLayout GroupLayout { get; }
        IProperty GetProperty(IPropertyFactory propertyType);
    }

}