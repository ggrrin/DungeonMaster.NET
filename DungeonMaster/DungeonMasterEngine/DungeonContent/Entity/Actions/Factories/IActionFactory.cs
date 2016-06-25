using DungeonMasterEngine.DungeonContent.GroupSupport;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions.Factories
{
    public interface IActionFactory
    {
        IAction CreateAction(ILiveEntity actionProvider);
    }
}