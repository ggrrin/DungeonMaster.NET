namespace DungeonMasterEngine.DungeonContent.Magic.Symbols.Factories
{
    public interface ISymbolFactory
    {
        string Name { get; }

        SpellSymbol Symbol { get; }
    }
}