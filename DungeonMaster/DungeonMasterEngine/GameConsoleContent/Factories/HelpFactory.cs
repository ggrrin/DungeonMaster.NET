using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.GameConsoleContent.Base;

namespace DungeonMasterEngine.GameConsoleContent.Factories
{
    public class HelpFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        public string CommandToken => "help";
        public string HelpText => $"usage:{CommandToken} [commandToken]";
        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new HelpCommand();
        public IParameterParser ParameterParser => null;

        public static HelpFactory Instance { get; } = new HelpFactory();

        private HelpFactory() { }
    }
}