using System;
using System.Threading.Tasks;
using DungeonMasterEngine.GameConsoleContent.Base;
using DungeonMasterEngine.Player;
using DungeonMasterEngine.Items;
using System.Collections.Generic;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class HandCommand : Interpreter
    {
        private Theron theron;

        public async override Task Run()
        {            
            theron = AppContext.Theron;

            if (Parameters.Length == 0)
            {
                if (theron.Hand != null)
                    Output.WriteLine(theron.Hand);
                else
                    Output.WriteLine("Empty");
            }
            else if(Parameters.Length == 1)
            {
                if(Parameters[0] == "take")
                {
                    var ch = await GetFromItemIndex(theron.PartyGroup);

                    if (ch != null)
                    {
                        theron.HandToInventory(ch);                        
                    }

                }
                else if(Parameters[0] == "put")
                {
                    var ch = await GetFromItemIndex(theron.PartyGroup);

                    if (ch != null)
                    {
                        var item = await GetFromItemIndex(ch.Inventory);
                        if (item != null)
                        {
                            theron.PutToHand(item, ch);
                        }                        
                    }
                }
                else if(Parameters[0] == "throw")
                {
                    theron.ThrowOutItem();                    
                }
            }
            else
            {
                Output.WriteLine("Invalid paramtetes");
            }
            
            
        } 


    }

    public class HandFactory : ICommandFactory<Dungeon>
    {
        private static HandFactory instance = new HandFactory();


        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static HandFactory Instance
        {
            get { return instance; }
        }

        private HandFactory()
        { }

        /// <summary>
        /// Text-form command
        /// </summary>
        /// <value>The command token.</value>
        public string CommandToken
        {
            get { return "hand"; }
        }

        /// <summary>
        /// Interpreter for command
        /// </summary>
        /// <value>The command interpreter.</value>
        public IInterpreter<Dungeon> GetNewInterpreter() => new HandCommand();

        /// <summary>
        /// Help text for command
        /// </summary>
        /// <value>The help text.</value>
        public string HelpText => "";


        public IParameterParser ParameterParser => null;
    }
}