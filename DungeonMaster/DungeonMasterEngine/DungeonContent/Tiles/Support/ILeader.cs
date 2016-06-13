using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public interface ILeader : ILocalizable<ITile>
    {
        IReadOnlyList<ILiveEntity> PartyGroup { get; }
        IGrabableItem Hand { get; set; }

        object Interactor { get; }
        ILiveEntity Leader { get; }
    }
}