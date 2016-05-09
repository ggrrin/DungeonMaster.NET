using System;
using System.Collections.Generic;

namespace DungeonMasterEngine.GameConsoleContent.Base
{
    /// <summary>
    /// Represents parser for transforming command text representaion into command interpreters
    /// </summary>
    public class CommandParser<Application>
    {
		private Dictionary<string, ICommandFactory<Application>> factories = new Dictionary<string, ICommandFactory<Application>>();

		/// <summary>
		/// Initialize parser with appropirate command factories
		/// </summary>
		/// <param name="ifactories">command factories</param>
		public CommandParser(IEnumerable<ICommandFactory<Application>> ifactories)
		{
			if (ifactories == null)
				throw new ArgumentNullException();

			foreach (var f in ifactories)
			{
				if (f == null)
					throw new ArgumentNullException("One of the factories is null.");

				if (factories.ContainsKey(f.CommandToken))
					throw new ArgumentException("Ambigious tokens for different factories: " + f.CommandToken);
				else
					factories.Add(f.CommandToken, f);
			}
		}

		/// <summary>
		/// Parser string commmand representaion and returns appropriate command interpreter
		/// or return null if command unrecognized
		/// </summary>
		/// <param name="command">command text representation</param>
		/// <returns>Return appropriate command interpreter or null if unrecognized</returns>
		public IInterpreter<Application> ParseCommand(string command)
		{
            command += " ";
            ICommandFactory<Application> factory;
            int tokenEnd = (command.TrimStart()).IndexOf(' ');
            if (tokenEnd > 0)
            {
                factories.TryGetValue(command.Trim().Substring(0, tokenEnd), out factory);
                if (factory != null)
                {
                    var interpreter = factory.GetNewInterpreter();
                    var ParamParser = factory.ParameterParser == null ? new DefaultParameterParser() : factory.ParameterParser;
                    interpreter.Parameters = ParamParser.ParseParameters(command.Trim().Substring(tokenEnd));
                    return interpreter;
                }
            }

			return null;
		}

	}
}
