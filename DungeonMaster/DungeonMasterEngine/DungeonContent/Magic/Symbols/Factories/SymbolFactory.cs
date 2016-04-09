namespace DungeonMasterEngine.DungeonContent.Magic.Symbols.Factories
{
    public class SymbolFactory<T> : ISymbolFactory where T:  SpellSymbol, new ()
    {
        public static ISymbolFactory Instance { get; } = new SymbolFactory<T>();

        private SymbolFactory() {}

        public string Name => Symbol.Name;

        public SpellSymbol Symbol { get; } = new T();
    }
}