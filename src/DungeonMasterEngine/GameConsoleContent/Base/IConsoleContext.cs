using System.Collections.Generic;

namespace DungeonMasterEngine.GameConsoleContent.Base
{
    public interface IConsoleContext<T>
    {
        IEnumerable<ICommandFactory<ConsoleContext<T>>> Factories { get; }
        T AppContext { get; }
    }
}