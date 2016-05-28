namespace DungeonMasterEngine.DungeonContent.EntitySupport.Attacks
{
    public interface IAttack
    {
        void ApplyAttack(MapDirection direction);
    }
}