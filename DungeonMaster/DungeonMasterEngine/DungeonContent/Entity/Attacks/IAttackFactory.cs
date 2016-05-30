using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Attacks
{
    public interface IAttackFactory
    {
        IAttack CreateAttackAction(IEntity attackProvider);
    }
}