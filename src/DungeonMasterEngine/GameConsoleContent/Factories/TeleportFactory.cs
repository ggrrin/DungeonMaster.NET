using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.GameConsoleContent.Base;

namespace DungeonMasterEngine.GameConsoleContent.Factories
{
    public class TeleportFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        public static TeleportFactory Instance { get; } = new TeleportFactory();

        public string CommandToken => "teleport";

        public string HelpText => $"usage: {CommandToken} x y\r\n x y are coordinates of tile";

        public IParameterParser ParameterParser => null;

        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new TeleportCommand();
    }
}