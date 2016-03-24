using DungeonMasterEngine.Interfaces;
using System;

namespace DungeonMasterEngine.Items
{
    public class NoConstrain : IConstrain
    {
        public bool IsAcceptable(object item)
        {
            return true;
        }
    }
}