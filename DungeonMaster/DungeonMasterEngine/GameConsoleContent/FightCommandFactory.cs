using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.GameConsoleContent.Base;

namespace DungeonMasterEngine.GameConsoleContent
{
    class FightCommandFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        public string CommandToken => "fight";
        public string HelpText => "usage: fight [<champoin_index>] [<action_index>] \r\n Apply given action by given champion";
        public IParameterParser ParameterParser => null;
        public static FightCommandFactory Instance { get; } = new FightCommandFactory();

        private FightCommandFactory() {}

        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new FightCommand();
    }
}