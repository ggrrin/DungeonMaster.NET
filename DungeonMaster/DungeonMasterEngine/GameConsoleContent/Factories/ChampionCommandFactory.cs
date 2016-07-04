using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.GameConsoleContent.Base;

namespace DungeonMasterEngine.GameConsoleContent.Factories
{
    public class ChampionCommandFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        private static ChampionCommandFactory instance = new ChampionCommandFactory();

        public static ChampionCommandFactory Instance => instance;

        private ChampionCommandFactory(){ }

        public string CommandToken => "champion"; 
        
        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new ChampionCommand();

        public string HelpText => $"{CommandToken} list\r\nlist party members\r\nif champion portrin is clicked interactive champion creation is executed.";

        public IParameterParser ParameterParser => null;
    }
}