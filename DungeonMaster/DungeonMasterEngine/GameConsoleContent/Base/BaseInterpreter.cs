// ***********************************************************************
// Assembly         : FTPClient
// Author           : ggrrin_
// Created          : 07-17-2015
//
// Last Modified By : ggrrin_
// Last Modified On : 07-26-2015
// ***********************************************************************
// <copyright file="ClientInterpreter.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DungeonMasterEngine.GameConsoleContent.Base
{
    /// <summary>
    /// Represents main interpreter to recognize client commands.
    /// </summary>
    public class BaseInterpreter : Interpreter
    {
        private CommandParser<ConsoleContext<Dungeon>> parser;

        private KeyboardStream inputStream;

        /// <summary>
        /// Initialize interpreter.
        /// </summary>
        /// <param name="factories">The factories.</param>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BaseInterpreter(IEnumerable<ICommandFactory<ConsoleContext<Dungeon>>> factories, TextReader input, TextWriter output, KeyboardStream inputStream)
        {
            if (factories == null || input == null || output == null || inputStream == null)
                throw new ArgumentNullException();

            Input = input;
            Output = output;
            this.inputStream = inputStream;

            parser = new CommandParser<ConsoleContext<Dungeon>>(factories);
        }

        public bool Running { get; set; } = true;

        public bool ExecutingCommand => RunningCommand != null;

        public IInterpreter<ConsoleContext<Dungeon>> RunningCommand { get; private set; }

        /// <summary>
        /// Runs the interpreter.
        /// </summary>
        /// <returns>Return s false whether application end is requested.</returns>
        public async override Task Run()
        {
            Output.WriteLine("Welcome! Write a command. (Consider using \"help\" command if U R lost.)");

            while (Running)
            {
                RunningCommand = await GetInterpreter();

                if (RunningCommand != null)
                {
                    RunningCommand.Input = Input;
                    RunningCommand.Output = Output;
                    RunningCommand.ConsoleContext = ConsoleContext;

                    await RunningCommand.Run();
                    RunningCommand = null;
                    Output.WriteLine();
                }
                else
                {
                    Output.WriteLine("Unrecognized command!");
                }
            }

            Output.WriteLine("Have a nice day.");
        }


        private async Task<IInterpreter<ConsoleContext<Dungeon>>> GetInterpreter()
        {
            var queueTask = WaitForImplicitCommand();
            var readLineTask = WaitForInput();

            var winTask = await Task.WhenAny(queueTask, readLineTask);
            if(winTask == queueTask)
            {
                inputStream.WriteLineToInput("Implicit command execution:");//To complete another task in order to be able to read in sub interpreter
                await readLineTask;   
            }

            return await winTask;
        }

        private async Task<IInterpreter<ConsoleContext<Dungeon>>> WaitForInput()
        {
            string command = await Input.ReadLineAsync();
            return parser.ParseCommand(command);
        }


        TaskCompletionSource<IInterpreter<ConsoleContext<Dungeon>>> interpreterPromise = new TaskCompletionSource<IInterpreter<ConsoleContext<Dungeon>>>();

        private async Task<IInterpreter<ConsoleContext<Dungeon>>> WaitForImplicitCommand()
        {
            var res = await interpreterPromise.Task;
            interpreterPromise = new TaskCompletionSource<IInterpreter<ConsoleContext<Dungeon>>>();
            return res;
        }

        public void RunCommand(IInterpreter<ConsoleContext<Dungeon>> interpreter)
        {
            interpreterPromise.SetResult(interpreter);//TODO try set result (maybe)
        }

    }
}

