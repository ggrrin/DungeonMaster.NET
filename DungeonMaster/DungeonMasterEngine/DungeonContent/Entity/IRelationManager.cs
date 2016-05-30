namespace DungeonMasterEngine.DungeonContent.Entity
{
    public interface IRelationManager
    {
        RelationToken RelationToken { get; }

        bool IsEnemy(RelationToken relationToken);
    }
}