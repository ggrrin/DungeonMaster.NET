using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Relations;

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