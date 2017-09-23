namespace DungeonMasterEngine.DungeonContent.Magic.Symbols
{
    public interface IPowerSymbol
    {
        string Name { get; }
        int ManaCostMultipler { get; }
        int LevelOrdinal { get; }
    }
}