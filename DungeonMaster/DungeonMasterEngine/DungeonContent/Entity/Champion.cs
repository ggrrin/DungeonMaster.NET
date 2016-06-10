using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public class Champion : LiveEntity
    {
        private readonly Animator<Champion, ISpaceRouteElement> animator = new Animator<Champion, ISpaceRouteElement>();

        private ISpaceRouteElement location;
        private string name;

        protected readonly IDictionary<IPropertyFactory, IProperty> properties;
        protected readonly IDictionary<ISkillFactory, ISkill> skills;

        public override float TranslationVelocity => 4.4f;
        public override IGroupLayout GroupLayout => Small4GroupLayout.Instance;
        public override IBody Body { get; } = new HumanBody();

        public override IProperty GetProperty(IPropertyFactory propertyType)
        {
            IProperty val;
            if (properties.TryGetValue(propertyType, out val))
                return val;
            else
                return new EmptyProperty();
        }

        public override ISkill GetSkill(ISkillFactory skillType)
        {
            ISkill val;
            if (skills.TryGetValue(skillType, out val))
                return val;
            else
                return new EmptySkill();
        } 

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                //((CubeGraphic)GraphicsProvider).Outter = true;
                //TODO
                //((CubeGraphic)GraphicsProvider).Texture = ResourceProvider.Instance.DrawRenderTarget(name, Color.Blue, Color.White);
            }
        }

        public string Title { get; set; }
        public bool IsMale { get; set; }

  

        public override IRelationManager RelationManager { get; }

        public override ISpaceRouteElement Location
        {
            get { return location; }
            set
            {
                if (location != null)
                {
                    animator.MoveTo(this, value, setLocation: false);
                }
                else//set Position at first
                {
                    if (!value.Tile.LayoutManager.TryGetSpace(this, value.Space))
                        throw new ArgumentException("Location is not accessable!");

                    Position = value.StayPoint;
                }
                location = value;
            }
        }

        public Champion(RelationToken relationToken, IEnumerable<RelationToken> enemiesRelationTokens, IChampionInitializator initializator ) : base(Vector3.Zero)
        {
            RelationManager = new DefaultRelationManager(relationToken, enemiesRelationTokens);
            var enumerable = initializator.GetProperties(this);
            var dictionary = enumerable.ToDictionary(p => p.Type);
            properties = dictionary;
            skills = initializator.GetSkills(this).ToDictionary(s => s.Factory);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            animator.Update(gameTime);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}