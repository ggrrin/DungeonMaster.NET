using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Relations;

namespace DungeonMasterEngine.Builders
{
    class CreatureInitializer : ICreatureInitializer {
        public int HitPoints { get; set; }
        public ISpaceRouteElement Location { get; set; }
        public RelationToken RelationToken { get; set; }
        public IEnumerable<RelationToken> EnemiesTokens { get; set; }
    }
}