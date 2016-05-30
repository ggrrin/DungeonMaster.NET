using System.Collections.Generic;
using DungeonMasterEngine.Builders.FloorActuatorFactories;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface IEntity : IMovable<ISpaceRouteElement> 
    {
        MapDirection FacingDirection { get; }
        IGroupLayout GroupLayout { get; }
        IRelationManager RelationManager { get; }
        IBody Body { get; }

        IProperty GetProperty(IPropertyFactory propertyType);
        ISkill GetSkill(ISkillFactory skillType);
    }
}