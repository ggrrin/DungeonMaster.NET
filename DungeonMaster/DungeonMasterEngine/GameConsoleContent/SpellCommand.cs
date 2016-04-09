using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Magic.Spells;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Magic.Symbols.Factories;
using DungeonMasterEngine.GameConsoleContent.Base;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class SpellCommand : Interpreter
    {
        private readonly SpellSymbolParser symbolParser;

        public SpellCommand()
        {
            symbolParser = new SpellSymbolParser( 
            new ISymbolFactory []
            {
                SymbolFactory<ZoSymbol>.Instance,
                SymbolFactory<OSymbol>.Instance
            },
            new ISpellFactory[]
            {
                OpenDoorSpellFactory.Instance
            });
        }

        public override async Task Run()
        {
            var theron = ConsoleContext.AppContext.Theron;
            var spellFactory = symbolParser.ParseSpell(Parameters);
            if (spellFactory != null)
                spellFactory.CastSpell(theron.Location, theron.GetShift(theron.ForwardDirection));
            else
                Output.WriteLine("Invalid spell symbols.");

            await Task.CompletedTask;
        }
    }

    public class SpellSymbolParser
    {
        public IEnumerable<ISymbolFactory> SymbolFactores { get; }
        public IEnumerable<ISpellFactory> SpellFactories { get; }

        public SpellSymbolParser(IEnumerable<ISymbolFactory> symbolFactores, IEnumerable<ISpellFactory> spellFactories)
        {
            SymbolFactores = symbolFactores;
            SpellFactories = spellFactories;
        }

        public ISpellFactory ParseSpell(IEnumerable<string> symbolParameters)
        {
            var symbolSequence = symbolParameters
                .Select(x => SymbolFactores.FirstOrDefault(y => string.Equals(x, y.Name, StringComparison.InvariantCultureIgnoreCase))?.Symbol)
                .ToArray();

            return symbolSequence.Any(x => x == null) ? null : SpellFactories.FirstOrDefault(x => x.CastingSequence.SequenceEqual(symbolSequence));
        }
    }

    public class SpellCommandFactory : ICommandFactory<ConsoleContext<Dungeon>>
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SpellCommandFactory Instance { get; } = new SpellCommandFactory();

        private SpellCommandFactory()
        { }

        /// <summary>
        /// Text-form command
        /// </summary>
        /// <value>The command token.</value>
        public string CommandToken => "spell";

        /// <summary>
        /// Interpreter for command
        /// </summary>
        /// <value>The command interpreter.</value>
        public IInterpreter<ConsoleContext<Dungeon>> GetNewInterpreter() => new SpellCommand();

        /// <summary>
        /// Help text for command
        /// </summary>
        /// <value>The help text.</value>
        public string HelpText => "usage: spell SYMBOL*";


        public IParameterParser ParameterParser => null;
    }
}
