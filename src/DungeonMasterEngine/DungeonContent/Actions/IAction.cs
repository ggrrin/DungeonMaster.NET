using DungeonMasterEngine.DungeonContent.Actions.Factories;

namespace DungeonMasterEngine.DungeonContent.Actions
{
    public interface IAction
    {
        IActionFactory Factory { get; }


        int Apply(MapDirection direction);
    }

    public interface IAction<out TFactory> : IAction where TFactory : IActionFactory
    {
        new TFactory Factory { get; }
    }
}