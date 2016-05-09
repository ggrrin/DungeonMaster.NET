using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.DungeonContent.Magic.Symbols.Factories;
using DungeonMasterEngine.GameConsoleContent.Base;

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
                spellFactory.CastSpell(theron.Location, theron.MapDirection);
            else
                Output.WriteLine("Invalid spell symbols.");

            await Task.CompletedTask;
        }
    }
}
