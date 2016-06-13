using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public abstract class LiveEntity : ILiveEntity
    {

        public abstract IGroupLayout GroupLayout { get; }

        public abstract IRelationManager RelationManager { get; }

        public abstract ISpaceRouteElement Location { get; set; }

        public MapDirection MapDirection { get; set; }

        public Vector3 Position { get; set; }
        public abstract float TranslationVelocity { get; }

        public virtual void Update(GameTime time) { }

        public abstract IBody Body { get; }

        public abstract IProperty GetProperty(IPropertyFactory propertyType);

        public abstract ISkill GetSkill(ISkillFactory skillType);
    }
}