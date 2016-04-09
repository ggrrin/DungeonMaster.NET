namespace DungeonMasterEngine.DungeonContent.Magic.Symbols
{
    public class ZoSymbol : SpellSymbol
    {
        public override uint ManaConst { get; } = 0;

        public override string Name => "Zo";
    }

    public class OSymbol : SpellSymbol
    {
        public override uint ManaConst { get; } = 1;

        public override string Name => "O";
    }
}