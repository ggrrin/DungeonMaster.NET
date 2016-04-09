using System.Text;
using System.Threading.Tasks;
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
}
