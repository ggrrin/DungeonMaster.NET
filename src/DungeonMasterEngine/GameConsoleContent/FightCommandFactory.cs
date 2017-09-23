using System;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.GameConsoleContent.Base;

namespace DungeonMasterEngine.GameConsoleContent
{
    class FightCommandFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        public string CommandToken => "fight";
        public string HelpText => "usage: fight [champoinIndex] [actionIndex]"  + Environment.NewLine +
            "without parameters: interactive selection of champion and his action" + Environment.NewLine +
            "with parameters: champion with index championIndex process action with index actionIndex";
        public IParameterParser ParameterParser => null;
        public static FightCommandFactory Instance { get; } = new FightCommandFactory();

        private FightCommandFactory() {}

        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new FightCommand();
    }
}