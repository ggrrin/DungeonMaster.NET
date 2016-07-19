using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.CreatureSpecific;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class DoorTile : DoorTile<Message>
    {
        public DoorTile(DoorInitializer initializer) : base(initializer) { }

    }

    public class DoorTile<TMessage> : FloorTile<TMessage>, IHasEntity where TMessage : Message
    {
        protected readonly Random rand = new Random();
        public Door Door { get; private set; }
        public IEntity Entity => Door.Open ? (IEntity)null : Door;

        public bool HasButton { get; private set; }
        public MapDirection Direction { get; private set; }

        private CancellationTokenSource lastCancellationToken;
        public bool Closing { get; set; }
        public override bool ContentActivated
        {
            get { return Door.Open; }
            protected set
            {
                lastCancellationToken?.Cancel(false);

                if (value)
                    Door.Open = value;
                else
                {
                    lastCancellationToken = new CancellationTokenSource();
                    TryClose(lastCancellationToken.Token);
                }
            }
        }

        private async void TryClose(CancellationToken token)
        {
            Closing = true;
            Door.Open = false;
            while (LayoutManager.Entities.Any())
            {
                if (token.IsCancellationRequested)
                {
                    Closing = false;
                    return;
                }

                F191_aayz_GROUP_GetDamageAllCreaturesOutcome(5);
                await Task.Delay(1000);
            }

            Closing = false;

        }

        public override bool IsAccessible => ContentActivated;

        public DoorTile(DoorInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(DoorInitializer initializer)
        {
            Direction = initializer.Direction;
            Door = initializer.Door;
            HasButton = initializer.HasButton;

            initializer.Initializing -= Initialize;
        }


        void F191_aayz_GROUP_GetDamageAllCreaturesOutcome(int P378_i_Attack)
        {
            int L0385_i_RandomAttack;
            P378_i_Attack -= (L0385_i_RandomAttack = (P378_i_Attack >> 3) + 1);
            L0385_i_RandomAttack <<= 1;

            foreach (var liveEntity in LayoutManager.Entities.ToArray())
            {
                if (liveEntity.GetProperty(PropertyFactory<NonMaterialProperty>.Instance).Value == 0)
                    liveEntity.GetProperty(PropertyFactory<HealthProperty>.Instance).Value -= P378_i_Attack + rand.Next(L0385_i_RandomAttack);
            }
        }

    }
}
