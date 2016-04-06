using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterEngine.GameConsoleContent.Base
{
    public class ConsoleContext<T> : IConsoleContext<T>
    {
        public IEnumerable<ICommandFactory<ConsoleContext<T>>> Factories { get; }
        public T AppContext { get; }

        public ConsoleContext(IEnumerable<ICommandFactory<ConsoleContext<T>>> factories , T applicationContext)
        {
            Factories = factories;
            AppContext = applicationContext;
        }
    }

    public interface IConsoleContext<T>
    {
        IEnumerable<ICommandFactory<ConsoleContext<T>>> Factories { get; }
        T AppContext { get; }
    }


    /// <summary>
    /// Interface for implementing interpreters and their transitive use.
    /// </summary>
    public interface IInterpreter<Application>
	{
		/// <summary>
		/// Input from which commands are read.
		/// Property should be set by creator of appropriate interpreter.
		/// </summary>
		TextReader Input { get; set; }

		/// <summary>
		/// Output to which commands result are written.
		/// Property should be set by creator of appropriate interpreter. 
		/// </summary>
		TextWriter Output { get; set; }

		/// <summary>
		/// Application context
		/// Property should be set by creator of appropriate interpreter.
		/// </summary>
		Application ConsoleContext { get; set; }

        string[] Parameters { get; set; }

        /// <summary>
        /// Determines wheather console can by minimized when command is in progress
        /// </summary>
        bool CanRunBackground { get; }

        /// <summary>
        /// Runs the interpreter
        /// </summary>
        /// <returns>Return s false whether application end is requested.</returns>
        Task Run();

	}
}
