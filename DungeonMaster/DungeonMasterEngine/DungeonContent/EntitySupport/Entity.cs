using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.EntitySupport
{
    public abstract class Entity : Item, IEntity
    {
        public MapDirection FacingDirection { get; set; }

        public abstract IGroupLayout GroupLayout { get; }

        public abstract IRelationManager RelationManager { get; }

        protected Entity(Vector3 position) : base(position) { }

        public new abstract ISpaceRouteElement Location { get; set; }

        public abstract float TranslationVelocity { get; }

        public abstract IBody Body { get; }

        public abstract IEntityProperty GetProperty(IEntityPropertyFactory propertyType);

        public abstract IEnumerable<IFightAction> FightActions { get; }
        public abstract CreatureInfo Type { get; }
    }
}