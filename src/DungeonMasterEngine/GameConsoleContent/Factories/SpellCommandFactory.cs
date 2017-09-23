using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.GameConsoleContent.Base;

namespace DungeonMasterEngine.GameConsoleContent.Factories
{
    public class SpellCommandFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SpellCommandFactory Instance { get; } = new SpellCommandFactory();

        private SpellCommandFactory()
        { }

        /// <summary>
        /// Text-form command
        /// </summary>
        /// <value>The command token.</value>
        public string CommandToken => "spell";

        /// <summary>
        /// Interpreter for command
        /// </summary>
        /// <value>The command interpreter.</value>
        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new SpellCommand();

        /// <summary>
        /// Help text for command
        /// </summary>
        /// <value>The help text.</value>
        public string HelpText => "usage: spell [championIndex symbolSequence]\r\n" +
             "without parameters: interactive spell cast \r\n" +
             "with parameters: champion with index championIndex cast spell defined by symbolSequence";


        public IParameterParser ParameterParser => null;
    }
}