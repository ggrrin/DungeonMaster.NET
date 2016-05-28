namespace DungeonMasterEngine.DungeonContent.EntitySupport
{
    public interface IRelationManager
    {
        RelationToken RelationToken { get; }

        bool IsEnemy(RelationToken relationToken);
    }
}