using DungeonMasterEngine.GameConsoleContent.Base;
using DungeonMasterEngine.Player;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actuators.WallSensors;
using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class ChampionCommand : Interpreter
    {
        public Sensor127 Actuator { get; set; }

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
                switch (parameter)
                {
                    case "list":
                        await GetFromItemIndex(ConsoleContext.AppContext.Leader.PartyGroup, false);
                        break;
                }
            }
            else
                Output.WriteLine("Invalid paramter");
        }

        private async Task ChampoinReincarnation()
        {
            Output.WriteLine("Champoin builder:");
            if (ConsoleContext.AppContext.Leader.PartyGroup.Count == 4)
            {
                Output.WriteLine("Group is full!!");
                return;
            }

            Output.WriteLine(Actuator.Champion);

            var getOption = new string[] { "Reincarnate", "rescue" };
            var res = await GetFromItemIndex(getOption);

            Champion champion = null;
            if (res == getOption[0])
                champion = await ReincarnateInterpreter();
            else if (res == getOption[1])
                champion = Actuator.Champion;
            else
                return;

            if (champion != null)
            {
                Actuator.Champion = null;
                ConsoleContext.AppContext.Leader.AddChampoinToGroup(champion);
                Output.WriteLine("Champoin succesfully added to group.");
            }
        }

        private async Task<Champion> ReincarnateInterpreter()
        {
            Output.WriteLine("Write first name.");
            string firstName = await Input.ReadLineAsync();

            Output.WriteLine("Write last name.");
            string lastName = await Input.ReadLineAsync();

            Output.WriteLine("Write title.");
            string title = await Input.ReadLineAsync();

            Actuator.Champion.Name = $"{firstName} {lastName}";
            Actuator.Champion.Title = title;
            return Actuator.Champion;

        }
    }
}
