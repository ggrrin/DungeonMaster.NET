using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public class Champion : LiveEntity, IRenderable
    {
        private readonly Animator<Champion, ISpaceRouteElement> animator = new Animator<Champion, ISpaceRouteElement>();

        private ISpaceRouteElement location;

        protected readonly IDictionary<IPropertyFactory, IProperty> properties;
        protected readonly IDictionary<ISkillFactory, ISkill> skills;

        public Renderer Renderer { get; set; }
        public override float TranslationVelocity => 4.4f;
        public override IGroupLayout GroupLayout => Small4GroupLayout.Instance;
        public override IBody Body { get; } = new HumanBody();

        public string Name { get; set; }
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

                //bool alreadyOnTile = location?.Tile == value.Tile;
                //if (!alreadyOnTile)
                //    location?.Tile?.OnObjectLeft(this);

                location = value;

                //if (!alreadyOnTile)
                //    location?.Tile?.OnObjectEntered(this);
            }
        }

        public Champion(IChampionInitializator initializator, RelationToken relationToken, IEnumerable<RelationToken> enemiesRelationTokens)
        {
            RelationManager = new DefaultRelationManager(relationToken, enemiesRelationTokens);
            var enumerable = initializator.GetProperties(this);
            var dictionary = enumerable.ToDictionary(p => p.Type);
            properties = dictionary;
            skills = initializator.GetSkills(this).ToDictionary(s => s.Factory);
        }

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


        public void Update(GameTime gameTime)
        {
            animator.Update(gameTime);
        }

        public override string ToString() => Name;

    }

    class ChampionRenderer : Renderer
    {
        private readonly CubeGraphic cube;
        public Champion Chamption { get; }

        public ChampionRenderer(Champion chamption, Texture2D face)
        {
            Chamption = chamption;

            cube = new CubeGraphic
            {
                Texture = face,
                Position = Chamption.Position,
                DrawFaces = CubeFaces.All,
                Outter = true,
                Scale = new Vector3(0.2f),
            };
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            cube.Position = Chamption.Position;
            cube.Draw(effect);
            return currentTransformation;
        }

        public override Matrix GetCurrentTransformation(ref Matrix parentTransformation)
        {
            return Matrix.Identity;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            return false;
        }
    }
}