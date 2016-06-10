using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public abstract class LiveEntity : Item, ILiveEntity
    {
        public MapDirection FacingDirection { get; set; }

        public abstract IGroupLayout GroupLayout { get; }

        public abstract IRelationManager RelationManager { get; }

        protected LiveEntity(Vector3 position) { } 

        public new abstract ISpaceRouteElement Location { get; set; }

        public abstract float TranslationVelocity { get; }

        public abstract IBody Body { get; }

        public abstract IProperty GetProperty(IPropertyFactory propertyType);

        public abstract ISkill GetSkill(ISkillFactory skillType);
    }
}