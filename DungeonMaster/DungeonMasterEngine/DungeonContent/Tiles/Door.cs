using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Tiles
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
            get { return Destroyed || open; }
            set
            {
                if (open != value)
                {
                    open = value;
                    OpenStateChanged?.Invoke(this, open);
                }
            }
        }

        public IRenderer Renderer { get; set; }

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
}