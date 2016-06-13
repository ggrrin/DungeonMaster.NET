namespace DungeonMasterParser.Descriptors
{
    public class DescriptorBase
    {
        public int Identifer { get; set; } = -1;
        public string Name { get; set; }
        public int Weight { get; set; }
        public string TexturePath { get; set; }

        public override string ToString() => $"{Identifer} {Name}";
    }
}