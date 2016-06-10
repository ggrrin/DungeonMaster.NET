namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public interface IFightAction
    {
        void Apply(ILiveEntity selfLiveEntity, ILiveEntity enemyLiveEntity);
    }
}