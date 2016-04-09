using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.GameConsoleContent.Base;

namespace DungeonMasterEngine.GameConsoleContent.Factories
{
    public class ItemFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        public static ItemFactory Instance { get; } = new ItemFactory();

        private ItemFactory() { }

        public string CommandToken => "item";

        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new ItemCommand();

        public string HelpText => $"{CommandToken} create NUMBER\r\n create fake item with identifer NUMBER and puts it to hand";

        public IParameterParser ParameterParser => null;
    }
}