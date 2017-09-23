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

        public string HelpText => $"usage: {CommandToken} list|sleep|switch\r\nlist: lists party members and for chosen one lists his statistics\r\nsleep: make party rest, for awake write wake|switch: switches champion positions in group";

        public IParameterParser ParameterParser => null;
    }
}