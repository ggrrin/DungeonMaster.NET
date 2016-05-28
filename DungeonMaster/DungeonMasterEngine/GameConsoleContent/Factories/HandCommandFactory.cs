using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.GameConsoleContent.Base;

namespace DungeonMasterEngine.GameConsoleContent.Factories
{
    public class HandCommandFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static HandCommandFactory Instance { get; } = new HandCommandFactory();

        private HandCommandFactory()
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
        public string HelpText => @"usage: hand [put|putsub|take|takesub|throw [DISTANCE]]
without parametres show content of hand
put: put item from specific inventory to hand 
putsub: put item from chest in inventory to hadn
take: take item from  hand to specific inventory
takesub: tak item from hand to chest in specific inventory
throw: item in hand on floor of current tile or specifed distance.";


        public IParameterParser ParameterParser => null;
    }
}