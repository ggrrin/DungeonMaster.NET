using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Magic;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.GameConsoleContent.Base;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class SpellCommand : Interpreter
    {
        public override async Task Run()
        {
            string championString = null;
            string[] symbols = null;
            switch (Parameters.Length)
            {
                case 0:
                    break;
                case 1:
                    championString = Parameters[0];
                    break;
                default:
                    championString = Parameters[0];
                    symbols = Parameters.Skip(1).ToArray();
                    break;
            }

            Champion champion = await GetChampion(championString);
            if (champion == null)
                return;

            var spellCastingManager = champion.SpellManager;


            if (symbols == null)
            {
                await ProcessSymbols(spellCastingManager, new AsyncEnumerator(this));
            }
            else
            {
                await ProcessSymbols(spellCastingManager, new ArrayEnumerator(symbols));
            }
        }


        private async Task ProcessSymbols(ISpellCastingManager manger, SymbolEnumerator e)
        {
            var factories = ConsoleContext.AppContext.Factories;

            bool clearSequence = true;
            if (manger.CurrentCastSequence.Any())
            {
                Output.WriteLine("Champion has already some symbols, do you want clear them? (Otherwise appended)[y/*]");
                if ("y" != await Input.ReadLineAsync())
                    clearSequence = false;
            }

            if (clearSequence)
            {
                manger.ClearCastingSequence();
                await e.MoveNext();

                var powerSymbol = factories.PowerSymbol.FirstOrDefault(x => x.Name.Equals(e.Current, StringComparison.InvariantCultureIgnoreCase));
                if (powerSymbol != null)
                {
                    if (!manger.TryBeginCastSpell(powerSymbol))
                    {
                        NotEnougMana();
                        return;
                    }
                }
                else
                {
                    Output.WriteLine($"Invalid power symbol name: {e.Current}");
                    if (!e.AllowCorrection)
                        return;
                }
            }

            while (await e.MoveNext())
            {
                var symbol = factories.SpellSymbols.FirstOrDefault(x => x.Name.Equals(e.Current, StringComparison.InvariantCultureIgnoreCase));
                if (symbol != null)
                {
                    if (!manger.TryCastSymbol(symbol))
                    {
                        NotEnougMana();
                        return;
                    }
                }
                else
                {
                    Output.WriteLine($"Invalid spell symbol name: {e.Current}");
                    if (!e.AllowCorrection)
                        return;
                }
            }

            if (!manger.TryCastSpell(factories.SpellFactories))
                Output.WriteLine($"Meaningless sequence or not enough practice!");
            else
            {
                Output.WriteLine($"Spell successfully casted!");
            }
        }

        private void NotEnougMana()
        {
            Output.WriteLine($"Not enough mana!");
        }

        private async Task<Champion> GetChampion(string championString)
        {
            var theron = ConsoleContext.AppContext.Leader;
            if (championString == null)
            {
                return await GetFromItemIndex(theron.PartyGroup);
            }
            else
            {
                int championIndex;
                bool res = int.TryParse(championString, out championIndex) &&
                    championIndex >= 0 && championIndex < theron.PartyGroup.Count;
                if (!res)
                {
                    Output.WriteLine("Invalid champion index.");
                    return null;
                }
                else
                {
                    return theron.PartyGroup[championIndex];
                }
            }
        }

        interface IAsyncEnumerator<T>
        {
            bool AllowCorrection { get; }
            Task<bool> MoveNext();

            T Current { get; }

        }

        public abstract class SymbolEnumerator : IAsyncEnumerator<string>
        {
            public abstract bool AllowCorrection { get; }

            public abstract Task<bool> MoveNext();

            public abstract string Current { get; protected set; }

            protected virtual bool IsFinish()
            {
                switch (Current)
                {
                    case "cast":
                        return false;
                    default:
                        return true;
                }
            }
        }

        class ArrayEnumerator : SymbolEnumerator
        {
            public ArrayEnumerator(IEnumerable<string> symbols)
            {
                Symbols = symbols.GetEnumerator();
            }

            public IEnumerator<string> Symbols { get; }

            public override bool AllowCorrection => false;

            public override async Task<bool> MoveNext()
            {
                await Task.CompletedTask;
                if (!Symbols.MoveNext())
                    return false;
                Current = Symbols.Current;
                return true;
            }

            public override string Current { get; protected set; }
        }

        public class AsyncEnumerator : SymbolEnumerator
        {
            public AsyncEnumerator(Interpreter interpreter)
            {
                Interpreter = interpreter;
            }

            public Interpreter Interpreter { get; }

            public override bool AllowCorrection => true;

            public override async Task<bool> MoveNext()
            {
                Interpreter.Output.WriteLine("Write symbol or cast it by \"cast\"");
                Current = await Interpreter.Input.ReadLineAsync();
                return IsFinish();
            }

            public override string Current { get; protected set; }
        }

    }
}
