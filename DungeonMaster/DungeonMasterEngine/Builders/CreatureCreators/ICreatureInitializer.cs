using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
using DungeonMasterEngine.DungeonContent.GrabableItems;

namespace DungeonMasterEngine.Builders.CreatureCreators
{
    public interface ICreatureInitializer
    {
        int HitPoints { get; set;  }
        ISpaceRouteElement Location { get; }
        RelationToken RelationToken { get; }
        IEnumerable<RelationToken> EnemiesTokens { get; }
        IEnumerable<IGrabableItem> PossessionItems { get; }
    }
}