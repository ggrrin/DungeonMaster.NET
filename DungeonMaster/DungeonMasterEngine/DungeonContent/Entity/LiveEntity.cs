using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
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

        public abstract void MoveTo(ITile newLocation, bool setNewLocation);

        public virtual void Update(GameTime time) { }

        public abstract IBody Body { get; }
        public virtual bool Activated { get; set; }

        public abstract IProperty GetProperty(IPropertyFactory propertyType);

        public abstract ISkill GetSkill(ISkillFactory skillType);
    }
}