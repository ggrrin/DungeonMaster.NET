

namespace DungeonMasterEngine.GameConsoleContent.Base
{
    /// <summary>
    /// Interface for implementation of Command factory
    /// </summary>
    public interface ICommandFactory<Application>
    {

		/// <summary>
		/// Text-form command
		/// </summary>
		string CommandToken { get; }

		/// <summary>
		/// Help text for command
		/// </summary>
		string HelpText { get; }

        IParameterParser ParameterParser { get; }


        /// <summary>
        /// Interpreter for command
        /// </summary>
        IInterpreter<Application> GetNewInterpreter();

	}
}
