using System;
using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Entity.Relations
{
    public class DefaultRelationManager : IRelationManager
    {
        public RelationToken RelationToken { get; }  
        protected readonly HashSet<RelationToken> enemyTokens = new HashSet<RelationToken>();

        public virtual bool IsEnemy(RelationToken relationToken) => enemyTokens.Contains(relationToken);

        public DefaultRelationManager(RelationToken token, IEnumerable<RelationToken> enemies)
        {
            RelationToken = token;
            foreach (var enemyToken in enemies)
            {
                if(enemyToken == token)
                    throw new InvalidOperationException("Entity cannot by enemy to itself.");
                enemyTokens.Add(enemyToken);
            }
        }
    }
}