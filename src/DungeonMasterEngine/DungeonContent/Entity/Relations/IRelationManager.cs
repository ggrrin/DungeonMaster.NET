namespace DungeonMasterEngine.DungeonContent.Entity.Relations
{
    public interface IRelationManager
    {
        RelationToken RelationToken { get; }

        bool IsEnemy(RelationToken relationToken);
    }
}