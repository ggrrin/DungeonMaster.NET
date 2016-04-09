using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.GameConsoleContent.Base;

namespace DungeonMasterEngine.GameConsoleContent.Factories
{
    public class HandFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static HandFactory Instance { get; } = new HandFactory();

        private HandFactory()
        { }

        /// <summary>
        /// Text-form command
        /// </summary>
        /// <value>The command token.</value>
        public string CommandToken => "hand";

        /// <summary>
        /// Interpreter for command
        /// </summary>
        /// <value>The command interpreter.</value>
        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new HandCommand();

        /// <summary>
        /// Help text for command
        /// </summary>
        /// <value>The help text.</value>
        public string HelpText => "usage: hand [put|take|throw [DISTANCE]|list|extract]\r\nwithout parametres show content of hand\r\n put: put item from specific inventory to hand \r\nthrow item in hand on floor of current tile or specifed distance\r\nIf item in hadn is container, it list its content\r\nIf item in hand is container it extract its contennt and add it to specifed inventory.";


        public IParameterParser ParameterParser => null;
    }
}