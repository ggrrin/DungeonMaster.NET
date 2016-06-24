using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.Builders
{
    class CreatureInitializer : ICreatureInitializer {
        public int HitPoints { get; set; }
        public ISpaceRouteElement Location { get; set; }
        public RelationToken RelationToken { get; set; }
        public IEnumerable<RelationToken> EnemiesTokens { get; set; }
    }
}