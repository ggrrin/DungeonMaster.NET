namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface IFightAction
    {
        void Apply(IEntity selfEntity, IEntity enemyEntity);
    }
}