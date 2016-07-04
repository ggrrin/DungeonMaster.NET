using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.Builders.ItemCreator;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.GrabableItems.Misc;
using DungeonMasterEngine.DungeonContent.Magic;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public class Champion : LiveEntity, IRenderable
    {
        protected readonly ChampionBones bones;
        protected readonly Animator<Champion, ISpaceRouteElement> animator = new Animator<Champion, ISpaceRouteElement>();

        protected ISpaceRouteElement location;

        protected readonly IDictionary<IPropertyFactory, IProperty> properties;
        protected readonly IDictionary<ISkillFactory, ISkill> skills;

        public event EventHandler<Champion> Died;
        public IRenderer Renderer { get; set; }
        public override float TranslationVelocity => 4.4f;
        public override IGroupLayout GroupLayout => Small4GroupLayout.Instance;
        public override IBody Body { get; } = new HumanBody();

        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsMale { get; set; }
        public ISpellCastingManager SpellManager { get; }

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
                        throw new ArgumentException("Location is not accessible!");

                    Position = value.StayPoint;
                }

                location = value;
            }
        }



        public Champion(IChampionInitializer initializator, RelationToken relationToken, IEnumerable<RelationToken> enemiesRelationTokens)
        {
            RelationManager = new DefaultRelationManager(relationToken, enemiesRelationTokens);
            var enumerable = initializator.GetProperties(this);
            properties = enumerable.ToDictionary(p => p.Type);
            skills = initializator.GetSkills(this).ToDictionary(s => s.Type);
            bones = initializator.BonesFactory.Create(new ChampionBonesInitializer { Champion = this });
            SpellManager = new SpellCastingManager(this);


            Activated = true;
            var health = (HealthProperty)properties[PropertyFactory<HealthProperty>.Instance];
            health.ValueChanged += (sender, value) =>
            {
                if (Activated && value <= 0)
                    Kill();
                else if (value > 0)
                    Activated = true;
                    
            };
        }

        private void Kill()
        {
            Activated = false;

            Died?.Invoke(this, this);
            location.Tile.LayoutManager.FreeSpace(this, location.Space);

            foreach (var storage in Body.Storages)
            {
                for (int i = 0; i < storage.Storage.Count; i++)
                {
                    var item = storage.TakeItemFrom(i);
                    if (item != null)
                        item.Location = GroupLayout.GetSpaceElement(Location.Space, location.Tile);
                }
            }
            bones.Location = GroupLayout.GetSpaceElement(Location.Space, location.Tile);
            location = null;
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

        public IEnumerable<IActionFactory> CurrentCombos
        {
            get { return Body.BodyParts.First(x => x.Type == ActionHandStorageType.Instance).Storage[0]?.FactoryBase.ActionCombo ?? Enumerable.Empty<IActionFactory>(); }
        }

        public override void MoveTo(ITile newLocation, bool setNewLocation)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            animator.Update(gameTime);
        }

        public override string ToString() => Name;

    }
}