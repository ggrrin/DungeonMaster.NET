namespace DungeonMasterEngine.DungeonContent.Entity.Attacks
{
    public interface IAttack
    {
        void ApplyAttack(MapDirection direction);
    }
}