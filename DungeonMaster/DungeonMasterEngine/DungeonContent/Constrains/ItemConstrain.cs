using System;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Items;

namespace DungeonMasterEngine.Items
{
    public class GrabableItemConstrain : IConstrain
    {
        public int Data { get; }

        public GrabableItemConstrain(int data)
        {
            Data = data;
        }

        public bool IsAcceptable(object item)
        {
            var i = item as GrabableItem;
            return i?.Identifer == Data;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {Data}";
        }
    }
}