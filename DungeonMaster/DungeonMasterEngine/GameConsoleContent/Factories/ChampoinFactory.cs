using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.GameConsoleContent.Base;

namespace DungeonMasterEngine.GameConsoleContent.Factories
{
    public class ChampoinFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        private static ChampoinFactory instance = new ChampoinFactory();

        public static ChampoinFactory Instance => instance;

        private ChampoinFactory(){ }

        public string CommandToken => "champion"; 
        
        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new ChampionCommand();

        public string HelpText => $"{CommandToken} list\r\nlist party members\r\nif champion portrin is clicked interactive champion creation is executed.";

        public IParameterParser ParameterParser => null;
    }
}