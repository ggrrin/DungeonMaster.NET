using System.Collections.Generic;
using DungeonMasterEngine.Builders.FloorActuatorFactories;
using DungeonMasterEngine.DungeonContent.EntitySupport;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties;
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

        IEntityProperty GetProperty(IEntityPropertyFactory propertyType);
        IEnumerable<IFightAction> FightActions { get; }
        CreatureInfo Type { get; }
    }
}