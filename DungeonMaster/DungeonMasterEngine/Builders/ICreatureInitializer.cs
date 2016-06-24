using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.Builders
{
    public interface ICreatureInitializer
    {
        int HitPoints { get; set;  }
        ISpaceRouteElement Location { get; }
        RelationToken RelationToken { get; }
        IEnumerable<RelationToken> EnemiesTokens { get; }
    }
}