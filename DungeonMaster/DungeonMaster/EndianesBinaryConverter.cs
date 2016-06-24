using System.IO;

namespace DungeonMasterParser
{
    class EndianesBinaryConverter : BigEndianBinaryReader
    {
        private readonly BinaryWriter writer;
        public EndianesBinaryConverter(Stream input) : base(input)
        {
            writer = new BinaryWriter(new FileStream("endianess.dat", FileMode.Create));
        }

        public override byte ReadByte()
        {
            var res = base.ReadByte();
            writer.Write(res);
            return res;
        }

        public override byte[] ReadBytes(int count)
        {
            var res = base.ReadBytes(count);
            writer.Write(res);
            return res;
        }

        public override ushort ReadUInt16()
        {
            var res = base.ReadUInt16();
            writer.Write(res);
            return res;
        }



        protected override void Dispose(bool disposing)
        {
            writer.Dispose();

            base.Dispose(disposing);
        }
    }
}