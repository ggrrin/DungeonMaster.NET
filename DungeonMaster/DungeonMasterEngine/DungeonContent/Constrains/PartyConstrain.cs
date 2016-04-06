using System;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;

namespace DungeonMasterEngine.Items
{
    internal class PartyConstrain : IConstrain
    {
        public bool IsAcceptable(object item)
        {
            var theron = item as Theron;
            return theron?.PartyGroup.Count == 4;
        }

        
    }
}