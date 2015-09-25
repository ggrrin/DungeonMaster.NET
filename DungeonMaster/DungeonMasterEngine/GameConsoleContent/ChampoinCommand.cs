using DungeonMasterEngine.GameConsoleContent.Base;
using DungeonMasterEngine.Items;
using DungeonMasterEngine.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class ChampoinCommand : Interpreter
    {

        public ChampoinActuator Actuator {get; set;}

        public override async Task Run()
        {
            if (Actuator != null)
                await ChampoinReincarnation();
            else
                await ChampoinInfo();
        }

        private async Task ChampoinInfo()
        {
            if (Parameters.Length > 0)
            {
                string parameter = Parameters[0];
                switch(parameter)
                {
                    case "list":
                        await GetFromItemIndex(AppContext.Theron.PartyGroup, false);
                        break;


                }
            }
            else
                Output.WriteLine("Invalid paramter");
        }

        private async Task ChampoinReincarnation()
        {
            Output.WriteLine("Champoin builder:");
            if (AppContext.Theron.PartyGroup.Count == 4)
            {
                Output.WriteLine("Group is full!!");
                return;
            }

            Output.WriteLine(Actuator.Champoin);

            var getOption = new string[] { "Reincarnate", "rescue" };
            var res = await GetFromItemIndex(getOption);

            Champoin champoin = null;
            if (res == getOption[0])
                champoin = await ReincarnateInterpreter();
            else if (res == getOption[1])
                champoin = Actuator.Champoin;
            else            
                return;
            

            if(champoin != null)
            {
                Actuator.RemoveChampoin();
                AppContext.Theron.PartyGroup.Add(champoin);
                Output.WriteLine("Champoin succesfully added to group.");
            }
        }

        private async Task<Champoin> ReincarnateInterpreter()
        {
            Output.WriteLine("Write first name.");
            string firstName = await Input.ReadLineAsync();

            Output.WriteLine("Write last name.");
            string lastName = await Input.ReadLineAsync();

            Output.WriteLine("Write title.");
            string title = await Input.ReadLineAsync();

            Actuator.Champoin.Name = $"{firstName} {lastName}";
            Actuator.Champoin.Title = title;
            return Actuator.Champoin;

        }
    }



    public class ChampoinFactory : ICommandFactory<Dungeon>
    {
        private static ChampoinFactory instance = new ChampoinFactory();


        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ChampoinFactory Instance
        {
            get { return instance; }
        }

        private ChampoinFactory()
        { }

        /// <summary>
        /// Text-form command
        /// </summary>
        /// <value>The command token.</value>
        public string CommandToken
        {
            get { return "champoin"; }
        }

        /// <summary>
        /// Interpreter for command
        /// </summary>
        /// <value>The command interpreter.</value>
        public IInterpreter<Dungeon> GetNewInterpreter() => new ChampoinCommand();

        /// <summary>
        /// Help text for command
        /// </summary>
        /// <value>The help text.</value>
        public string HelpText => "";


        public IParameterParser ParameterParser => null;
    }
}
