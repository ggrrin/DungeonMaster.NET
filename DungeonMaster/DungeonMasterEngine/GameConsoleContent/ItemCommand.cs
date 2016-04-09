using DungeonMasterEngine.GameConsoleContent.Base;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Items;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class ItemCommand : Interpreter
    {
        private Theron theron;

        public override async Task Run()
        {
            theron = ConsoleContext.AppContext.Theron;

            if (Parameters.Length > 0)
            {
                switch (Parameters[0])
                {
                    case "create":
                        int identifer = -1;
                        if (Parameters.Length == 2 && int.TryParse(Parameters[1], out identifer))
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
                        else
                        {
                            Output.WriteLine("Invalid Parmeter");
                        }
                        break;
                }
            }
        }

    }
}
