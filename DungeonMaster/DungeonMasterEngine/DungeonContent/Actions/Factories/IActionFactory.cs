using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.DungeonContent.Actions.Factories
{
    public interface IActionFactory
    {
        IAction CreateAction(ILiveEntity actionProvider);
    }

    //public interface IActionFactory<out TAction> : IActionFactory where TAction : IAction
    //{
    //    new TAction CreateAction(ILiveEntity actionProvider);
    //}
}