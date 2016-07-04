using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
using DungeonMasterEngine.DungeonContent.GrabableItems;

namespace DungeonMasterEngine.Builders.CreatureCreator
{
    class CreatureInitializer : ICreatureInitializer {
        public int HitPoints { get; set; }
        public ISpaceRouteElement Location { get; set; }
        public RelationToken RelationToken { get; set; }
        public IEnumerable<RelationToken> EnemiesTokens { get; set; }
        public IEnumerable<IGrabableItem> PossessionItems { get; set; }
    }
}