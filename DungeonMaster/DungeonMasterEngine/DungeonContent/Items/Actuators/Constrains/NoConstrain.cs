using DungeonMasterEngine.Interfaces;
using System;

namespace DungeonMasterEngine.Items
{
    public class NoConstrain : IConstrain
    {
        public bool IsAcceptable(GrabableItem item)
        {
            return true;
        }
    }
}