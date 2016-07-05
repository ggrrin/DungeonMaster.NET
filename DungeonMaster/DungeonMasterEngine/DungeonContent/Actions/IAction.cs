using DungeonMasterEngine.DungeonContent.Actions.Factories;

namespace DungeonMasterEngine.DungeonContent.Actions
{
    public interface IAction
    {
        IActionFactory Factory { get; }
        int ApplyAttack(MapDirection direction);
    }

    public interface IAction<out TFactory> : IAction where TFactory : IActionFactory
    {
        new TFactory Factory { get; }
    }
}