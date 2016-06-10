using System;
using System.Collections.Generic;
using System.Net;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public class Door : IRenderable, IEntity
    {
        private readonly Dictionary<IPropertyFactory,IProperty> properties;
        private bool open;
        public IGroupLayout GroupLayout => FullTileLayout.Instance;
        public bool Destroyed => GetProperty(PropertyFactory<HealthProperty>.Instance).Value <= 0;
        public bool CanItemsFly { get; }
        public bool IsTransparent { get; } 

        public bool Open
        {
            get { return open; }
            set
            {
                if (open != value)
                {
                    open = value;
                    OpenStateChanged?.Invoke(this, open);
                }
            }
        }

        public Renderer Renderer { get; set; }

        public event EventHandler<bool> OpenStateChanged;

        public Door(bool open, int defense, bool meleAttackDestructible, bool magicAttackDestructible, bool canItemsFly, bool isTransparent)
        {
            Open = open;
            CanItemsFly = canItemsFly;
            IsTransparent = isTransparent;

            properties = new Dictionary<IPropertyFactory, IProperty>(); 
            SetupProperties(defense, meleAttackDestructible, magicAttackDestructible);
        }

        private void SetupProperties(int defense, bool meleAttackDestructible, bool magicAttackDestructible)
        {
            var health = new HealthProperty(defense);
            var antiMele = new DefenseProperty(meleAttackDestructible ? 0 : 1000000);//TODO thinkout
            var antiMagic = new AntiMagicProperty(magicAttackDestructible ? 0 : 1000000);//TODO thinkout

            properties.Add(health.Type, health);
            properties.Add(antiMele.Type, antiMele);
            properties.Add(antiMagic.Type, antiMagic);
        }

        bool F232_dzzz_GROUP_IsDoorDestroyedByAttack(int P506_i_Attack, bool P507_B_MagicAttack, int P508_i_Ticks)
        {
            //if ((P507_B_MagicAttack && !MagicDestructible) ||
            //    (!P507_B_MagicAttack && !MeleeDestructible))
            //{
            //    return false;
            //}

            //if (P506_i_Attack >= Defense)
            //{
            //    if (Closed)
            //    {
            //        Destroyed = true;
            //        return true;
            //    }
            //}
            return false;
        }


        public IProperty GetProperty(IPropertyFactory propertyType)
        {
            IProperty res;
            properties.TryGetValue(propertyType, out res);
            return res ?? new EmptyProperty();
        }
    }

    public class DoorRenderer : TextureRenderer
    {
        public DoorRenderer(Texture2D doorTexture) : base(Matrix.CreateScale(2 / 3f) * Matrix.CreateTranslation(new Vector3(0, -1 / 6f, 0.3f)), doorTexture) { }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            base.Render(ref currentTransformation, effect, parameter);
            var finalmatrix = Matrix.CreateRotationY(MathHelper.Pi) * currentTransformation;
            return base.Render(ref finalmatrix, effect, parameter);
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            return false;
        }
    }
}