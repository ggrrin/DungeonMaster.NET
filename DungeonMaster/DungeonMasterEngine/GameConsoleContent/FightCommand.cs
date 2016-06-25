using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.GameConsoleContent.Base;
using DungeonMasterEngine.Player;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class FightCommand : Interpreter
    {
        private T GetItemAt<T>(IReadOnlyList<T> list, int parameterIndex) 
        {
            int parameterValue;

            if (list != null && int.TryParse(Parameters[parameterIndex], out parameterValue) && parameterValue >= 0 && parameterValue < list.Count)
            {
                return list[parameterValue];
            }
            else
            {
                return default(T);
            }
        }

        public override async Task Run()
        {
            Theron t = ConsoleContext.AppContext.Leader;
            Champion champoin = null;
            IActionFactory action = null;
            if (Parameters.Length == 0)
            {
                champoin = await GetFromItemIndex(t.PartyGroup);
                action = await GetFromItemIndex(champoin.CurrentCombos.ToArray());
            }
            else if (Parameters.Length == 1)
            {
                champoin = GetItemAt(t.partyGoup, 0);
                action = await GetFromItemIndex(champoin?.CurrentCombos.ToArray());

            }
            else if (Parameters.Length == 2)
            {
                champoin = GetItemAt(t.partyGoup, 0);
                action = GetItemAt(champoin?.CurrentCombos.ToArray(), 1);
            }

            if (champoin != null && action != null)
            {
                Fight(t, champoin, action);
            }
            else
            {
                Output.WriteLine("Invalid arguments.");
            }
        }

        public void Fight(Theron theron, ILiveEntity champion, IActionFactory action)
        {
            action.CreateAction(champion).ApplyAttack(theron.MapDirection);
        }
    }
}
