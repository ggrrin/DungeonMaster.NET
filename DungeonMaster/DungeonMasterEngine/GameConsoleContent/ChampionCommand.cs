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
    public class ChampionCommand : Interpreter
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
                        await GetFromItemIndex(ConsoleContext.AppContext.Theron.PartyGroup, false);
                        break;


                }
            }
            else
                Output.WriteLine("Invalid paramter");
        }

        private async Task ChampoinReincarnation()
        {
            Output.WriteLine("Champoin builder:");
            if (ConsoleContext.AppContext.Theron.PartyGroup.Count == 4)
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
                ConsoleContext.AppContext.Theron.PartyGroup.Add(champoin);
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

    public class ChampoinFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        private static ChampoinFactory instance = new ChampoinFactory();

        public static ChampoinFactory Instance => instance;

        private ChampoinFactory(){ }

        public string CommandToken => "champion"; 
        
        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new ChampionCommand();

        public string HelpText => $"{CommandToken} list\r\nlist party members\r\nif champion portrin is clicked interactive champion creation is executed.";

        public IParameterParser ParameterParser => null;
    }
}
