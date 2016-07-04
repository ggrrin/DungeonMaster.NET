namespace DungeonMasterEngine.DungeonContent.Magic.Symbols
{
    public interface IPowerSymbol
    {
        string Name { get; }
        int ManaCostMultipler { get; }
        int LevelOrdinal { get; }
    }

    public class PowerSymbol : IPowerSymbol
    {
        public string Name { get; }
        public int ManaCostMultipler { get; }
        public int LevelOrdinal { get; }

        public PowerSymbol(string name, int manaCostMultipler, int levelOrdinal)
        {
            ManaCostMultipler = manaCostMultipler;
            LevelOrdinal = levelOrdinal;
            Name = name;
        }
    }
}