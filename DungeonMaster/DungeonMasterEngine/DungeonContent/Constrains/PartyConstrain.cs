using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;

namespace DungeonMasterEngine.DungeonContent.Constrains
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