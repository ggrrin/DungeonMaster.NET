using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.EntitySupport.Attacks
{
    public interface IAttackFactory
    {
        IAttack CreateAttackAction(IEntity attackProvider);
    }
}