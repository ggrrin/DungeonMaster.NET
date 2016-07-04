using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;

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

        protected async Task<T> GetFromItemIndex<T>(IEnumerable<T> list, bool selectItem = true) where T : class
        {
            Output.WriteLine("Select index:");
            int j = 0;
            foreach (var i in list)
                Output.WriteLine($"{j++} {i?.ToString() ?? "--slot_empty--"}");

            if (selectItem)
            {
                int index = -1;
                string s = await Input.ReadLineAsync();
                if (int.TryParse(s, out index) && index >= 0 && index < list.Count())
                    return list.ElementAt(index);
                else
                {
                    Output.WriteLine("Invalid index!");
                    return null;
                }
            }
            else
                return default(T);
        }
        protected async Task<int?> GetItemIndex<T>(IEnumerable<T> list, bool result = true) where T : class
        {
            Output.WriteLine("Selecet index:");
            int j = 0;
            foreach (var i in list)
                Output.WriteLine($"{j++} {i?.ToString() ?? "--slot_empty--"}");

            if (result)
            {
                int index = -1;
                if (int.TryParse(await Input.ReadLineAsync(), out index) && index >= 0 && index < list.Count())
                    return index;
                else
                {
                    Output.WriteLine("Invalid index!");
                    return null;
                }
            }
            else
                return null;
        }
    }
}