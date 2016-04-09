using DungeonMasterEngine.GameConsoleContent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class HelpCommand : Interpreter
    {
        public override async Task Run()
        {
            if (!Parameters.Any())
            {
                foreach (var cmd in ConsoleContext.Factories)
                    Output.WriteLine(cmd.CommandToken);

                Output.WriteLine("For more information write \"help command\"");
            }
            else if (Parameters.Length == 1)
            {
                var factory = ConsoleContext.Factories.SingleOrDefault(x => string.Equals(x.CommandToken, Parameters[0], StringComparison.InvariantCultureIgnoreCase));
                if (factory != null)
                    Output.WriteLine(factory.HelpText);
                else
                    Output.WriteLine($"Command \"{Parameters[0]}\" doesnt exists");
            }
            else
                Output.WriteLine("Invalid parameter");
        }
    }

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
