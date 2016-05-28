using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.EntitySupport;
using DungeonMasterEngine.DungeonContent.EntitySupport.BodyInventory;
using DungeonMasterEngine.DungeonContent.EntitySupport.Properties;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Player
{
    public class Champion : Entity
    {
        private readonly Animator<Champion, ISpaceRouteElement> animator = new Animator<Champion, ISpaceRouteElement>();

        private ISpaceRouteElement location;
        private string name;

        private IEntityProperty[] properties = new IEntityProperty[]
        {
            
        };


        public override float TranslationVelocity => 4.4f;
        public override IGroupLayout GroupLayout => Small4GroupLayout.Instance;
        public override IBody Body { get; } = new HumanBody();

        public override IEntityProperty GetProperty(IEntityPropertyFactory propertyType) => properties.FirstOrDefault(p => p.Type == propertyType) ?? new EmptyProperty();

        public override IEnumerable<IFightAction> FightActions { get; }
        public override CreatureInfo Type { get; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                ((CubeGraphic)GraphicsProvider).Outter = true;
                ((CubeGraphic)GraphicsProvider).Texture = ResourceProvider.Instance.DrawRenderTarget(name, Color.Blue, Color.White);
            }
        }

        public string Title { get; set; }
        public bool IsMale { get; set; }

        public int Health { get; set; }
        public int Stamina { get; set; }
        public int Mana { get; set; }

        public int Luck { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Vitality { get; set; }


        public int Wisdom { get; set; }
        public int AntiMagic { get; set; }
        public int AntiFire { get; set; }

        public int Fighter { get; set; }
        public int Ninja { get; set; }
        public int Priest { get; set; }
        public int Wizard { get; set; }

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

        public Champion(RelationToken relationToken, IEnumerable<RelationToken> enemiesRelationTokens) : base(Vector3.Zero)
        {
            RelationManager = new DefaultRelationManager(relationToken, enemiesRelationTokens);
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