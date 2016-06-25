namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public interface IAction
    {
        void ApplyAttack(MapDirection direction);
    }
}