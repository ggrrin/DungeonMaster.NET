using System;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Items;

namespace DungeonMasterEngine.Items
{
    public class ItemConstrain : IConstrain
    {
        public int Data { get; }

        public ItemConstrain(int data)
        {
            Data = data;
        }

        public bool IsAcceptable(GrabableItem item)
        {
            return item?.Identifer == Data;
        }
    }
}