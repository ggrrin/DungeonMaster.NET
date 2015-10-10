using DungeonMasterEngine.GameConsoleContent.Base;
using DungeonMasterEngine.Items;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class ItemCommand : Interpreter
    {
        private Theron theron;

        public override async Task Run()
        {
            theron = AppContext.Theron;

            if(Parameters.Length > 0)
            {
                switch(Parameters[0])
                {
                    case "create":
                        if(Parameters.Length > 1)
                        {
                            int identifer = -1;
                            if(int.TryParse(Parameters[1], out identifer))
                            {
                                if (theron.Hand != null)
                                    Output.WriteLine("Hand is not empty.");
                                else
                                {
                                    var item = new Miscellaneous(Vector3.Zero);

                                    item.Identifer = identifer;
                                    item.Name = "Artifical fake item";
                                    theron.PutToHand(item, null);
                                    Output.Write($"Item: {item} added to hand.");

                                }
                            }
                        }
                        break;
                }
            }
        }

    }

    public class ItemFactory : ICommandFactory<Dungeon>
    {
        private static ItemFactory instance = new ItemFactory();

        public static ItemFactory Instance => instance;

        private ItemFactory() { }

        public string CommandToken => "item";

        public IInterpreter<Dungeon> GetNewInterpreter() => new ItemCommand();

        public string HelpText => "";

        public IParameterParser ParameterParser => null;
    }
}
