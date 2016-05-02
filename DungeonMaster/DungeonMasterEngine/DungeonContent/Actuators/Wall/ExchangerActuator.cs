using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public class ExchangerActuator : Actuator
    {
        private bool used = false;
        private GrabableItem Storage;
        private Texture2D decorationDeactived;
        private Texture2D decorationActivated;

        public bool IsEmpty => Storage == null;

        public bool OnceOnly { get; }

        public IConstrain Constrain { get; }

        public Texture2D DecorationDeactived
        {
            get { return decorationDeactived; }
            set
            {
                decorationDeactived = value;
                UpdateDecoration();
            }
        }

        public Texture2D DecorationActivated
        {
            get { return decorationActivated; }
            set
            {
                decorationActivated = value;
                UpdateDecoration();
            }
        }

        public ExchangerActuator(Vector3 position, GrabableItem storage, IConstrain exchangeConstrain, bool onceOnly) : base(position)
        {
            Constrain = exchangeConstrain;
            Storage = storage;
            OnceOnly = onceOnly;
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if (OnceOnly && used)
                return item;

            if (item == null || Constrain.IsAcceptable(item))
            {
                var res = Storage;
                Storage = item;

                UpdateDecoration();

                used = true;

                return res;
            }
            else
            {
                return item;
            }
        }

        public void UpdateDecoration() => ((CubeGraphic) Graphics).Texture = IsEmpty ? DecorationDeactived : DecorationActivated;
    }
}