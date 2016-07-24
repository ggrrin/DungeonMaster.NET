using System;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{

    public class Pit : Pit<Message>
    {
        public Pit(PitInitializer initializer) : base(initializer) { }
    }

    public class Pit<TMessage> : FloorTile<TMessage>, ILevelConnector where TMessage : Message
    {
        private ITile nextLevelEnter;

        public ITile NextLevelEnter
        {
            get { return nextLevelEnter; }
            set
            {
                nextLevelEnter = value;
                PitNeighbors.Down = value;
            }
        }

        public int NextLevelIndex => LevelIndex + 1;
        public Point TargetTilePosition => GridPosition;


        public PitTileNeighbors PitNeighbors => (PitTileNeighbors)Neighbors;

        public override TileNeighbors Neighbors
        {
            get { return base.Neighbors; }
            protected set
            {
                base.Neighbors = (PitTileNeighbors)value;
            }
        }

        public bool Invisible { get; private set; }
        public bool Imaginary { get; private set; }

        public bool IsOpen => ContentActivated;
        public override bool IsDangerous => IsOpen; 

        public override bool IsAccessible => true;

        public override bool ContentActivated
        {
            get { return base.ContentActivated; }
            protected set
            {
                base.ContentActivated = value;
                foreach (var subItem in SubItems.ToArray())
                    MakeItemFall(subItem);

            }
        }

        public override void OnObjectEntered(object localizable)
        {
            base.OnObjectEntered(localizable);

            MakeItemFall(localizable);

        }

        private async void MakeItemFall(object item)
        {
            if (IsOpen)
            {
                var entities = LayoutManager.Entities.ToArray();
                var movable = item as IMovable<ISpaceRouteElement>;
                if (movable != null)
                {
                    await movable.MoveToAsync(movable.Location.GetNew(PitNeighbors.Down));
                }
                else
                {
                    var localizable = item as ILocalizable<ISpaceRouteElement>;
                    if(localizable != null)
                        localizable.Location = localizable.Location.GetNew(PitNeighbors.Down);
                }

                foreach (var entity in entities)
                    F324_aezz_CHAMPION_DamageAll_GetDamagedChampionCount(entity, 20); //, MASK0x0010_LEGS | MASK0x0020_FEET, C2_ATTACK_SELF)
            }
        }

        private static readonly Random rand = new Random();

        void F324_aezz_CHAMPION_DamageAll_GetDamagedChampionCount(ILiveEntity entity, int P669_ui_Attack)//, int P670_i_Wounds, int P671_i_AttackType)
        {
            int L0984_i_ChampionIndex;
            int L0985_i_RandomAttack;
            int L0986_i_DamagedChampionCount;

            
            //customized 
            L0985_i_RandomAttack = (P669_ui_Attack >> 3) + 1;
            P669_ui_Attack += (rand.Next(3) - 1) * L0985_i_RandomAttack;

            entity.GetProperty(PropertyFactory<HealthProperty>.Instance).Value -= P669_ui_Attack;


            //L0985_i_RandomAttack <<= 1;
            //for (L0986_i_DamagedChampionCount = 0, L0984_i_ChampionIndex = C00_CHAMPION_FIRST; L0984_i_ChampionIndex < G305_ui_PartyChampionCount; L0984_i_ChampionIndex++)
            //{
            //    if (F321_AA29_CHAMPION_AddPendingDamageAndWounds_GetDamage(L0984_i_ChampionIndex, F025_aatz_MAIN_GetMaximumValue(1, P669_ui_Attack + M02_RANDOM(L0985_i_RandomAttack)), P670_i_Wounds, P671_i_AttackType))
            //    { /* Actual attack is P669_ui_Attack +/- (P669_ui_Attack / 8) */
            //        L0986_i_DamagedChampionCount++;
            //    }
            //}
            //return L0986_i_DamagedChampionCount;
        }

        public Pit(PitInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(PitInitializer initializer)
        {
            ContentActivated = initializer.IsOpen;
            Invisible = initializer.Invisible;
            Imaginary = initializer.Imaginary;

            initializer.Initializing -= Initialize;
        }

    }
}
