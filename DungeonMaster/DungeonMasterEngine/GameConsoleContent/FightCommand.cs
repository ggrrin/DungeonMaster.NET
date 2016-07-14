using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity;
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

        private bool Ready(Champion ch)
        {
            if (ch == null)
                return false;

            if (!ch.ReadyForAction)
            {
                Output.WriteLine("Champion not ready!");
                return false;
            }
            return true;
        }

        public override async Task Run()
        {
            LegacyLeader t = ConsoleContext.AppContext.Leader;
            Champion champoin = null;
            IActionFactory action = null;
            if (Parameters.Length == 0)
            {
                champoin = await GetFromItemIndex(t.PartyGroup);
                if (!Ready(champoin))
                    return;
                
                action = await GetFromItemIndex(champoin.CurrentCombos.ToArray());
            }
            else if (Parameters.Length == 1)
            {
                champoin = GetItemAt(t.partyGroup, 0);
                if (!Ready(champoin))
                    return;
                action = await GetFromItemIndex(champoin?.CurrentCombos.ToArray());

            }
            else if (Parameters.Length == 2)
            {
                champoin = GetItemAt(t.partyGroup, 0);
                if (!Ready(champoin))
                    return;
                action = GetItemAt(champoin?.CurrentCombos.ToArray(), 1);
            }

            if (champoin != null && action != null)
                champoin.DoAction(action);
            else
                Output.WriteLine("Invalid arguments.");
        }

    }
}
