using System.Collections.Generic;
using DungeonMasterEngine.Builders.FloorActuatorFactories;
using DungeonMasterEngine.DungeonContent.EntitySupport;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties.@base;
using DungeonMasterEngine.DungeonContent.EntitySupport.Skills;
using DungeonMasterEngine.DungeonContent.EntitySupport.Skills.@base;
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
        ISkill GetSkill(ISkillFactory propertyType);
        IEnumerable<IFightAction> FightActions { get; }
        CreatureInfo Type { get; }
    }
}