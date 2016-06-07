using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public interface ILeader
    {

        IReadOnlyList<IEntity> PartyGroup { get; }
        IGrabableItem Hand { get; set; }

        object Interactor { get; }
        IEntity Leader { get; }
    }
}