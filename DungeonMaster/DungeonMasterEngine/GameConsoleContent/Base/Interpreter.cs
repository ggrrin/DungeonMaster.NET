using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DungeonMasterEngine.GameConsoleContent.Base
{
    public abstract class Interpreter : IInterpreter<ConsoleContext<Dungeon>>
    {
        public ConsoleContext<Dungeon> ConsoleContext { get; set; }

        public virtual bool CanRunBackground => false;
        public TextReader Input { get; set; }

        public TextWriter Output { get; set; }

        public string[] Parameters { get; set; }

        public abstract Task Run();

        protected async Task<T> GetFromItemIndex<T>(IList<T> list, bool result = true) where T : class
        {
            Output.WriteLine("Selecet index:");
            int j = 0;
            foreach (var i in list)
                Output.WriteLine($"{j++} {i}");

            if (result)
            {
                int index = -1;
                if (int.TryParse(await Input.ReadLineAsync(), out index) && index >= 0 && index < list.Count)
                    return list[index];
                else
                {
                    Output.WriteLine("Invalid index!");
                    return null;
                }
            }
            else
                return default(T);
        }
    }
}