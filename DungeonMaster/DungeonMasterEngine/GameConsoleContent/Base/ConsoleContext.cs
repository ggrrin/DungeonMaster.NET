using System.Collections.Generic;

namespace DungeonMasterEngine.GameConsoleContent.Base
{
    public class ConsoleContext<T> : IConsoleContext<T>
    {
        public IEnumerable<ICommandFactory<ConsoleContext<T>>> Factories { get; }
        public T AppContext { get; }

        public ConsoleContext(IEnumerable<ICommandFactory<ConsoleContext<T>>> factories , T applicationContext)
        {
            Factories = factories;
            AppContext = applicationContext;
        }
    }
}