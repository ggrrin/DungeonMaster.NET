namespace DungeonMasterEngine.DungeonContent.Magic.Symbols
{
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