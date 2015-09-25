using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Items;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Graphics;

namespace DungeonMasterEngine.Items
{
    public class ExchangerActuator : Actuator
    {
        GrabableItem Storage;

        public bool IsEmpty { get { return Storage == null; } }

        public IConstrain Constrain { get; }
        public Texture2D DecorationDeactived { get;  set; }
        public Texture2D DecorationActivated { get; set; }
        public ExchangerActuator(Vector3 position, GrabableItem storage, IConstrain exchangeConstrain) : base(position)
        {
            Constrain = exchangeConstrain;
            Storage = storage;
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            if (item == null || Constrain.IsAcceptable(item))
            {
                var res = Storage;
                Storage = item;

                UpdateDecoration();

                return res;
            }
            else
            {
                return item;
            }
        }

        public void UpdateDecoration()
        {//TODO call in setters
            if (IsEmpty)
                (Graphics as CubeGraphic).Texture = DecorationDeactived;
            else
                (Graphics as CubeGraphic).Texture = DecorationActivated;
        }
    }
}