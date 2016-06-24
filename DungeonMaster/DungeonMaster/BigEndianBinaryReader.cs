using System.IO;

namespace DungeonMasterParser
{
    class BigEndianBinaryReader : BinaryReader
    {
        public BigEndianBinaryReader(Stream input) : base(input)
        {

        }

        public override ushort ReadUInt16()
        {
            var b = base.ReadUInt16();
            ushort res = (ushort)((b << 8) + (b >> 8));
            return res;
        }
    }
}