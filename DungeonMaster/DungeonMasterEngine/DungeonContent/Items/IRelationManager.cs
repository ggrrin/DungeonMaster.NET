namespace DungeonMasterEngine.DungeonContent.Items
{
    public interface IRelationManager
    {
        RelationToken RelationToken { get; }

        bool IsEnemy(RelationToken relationToken);
    }
}