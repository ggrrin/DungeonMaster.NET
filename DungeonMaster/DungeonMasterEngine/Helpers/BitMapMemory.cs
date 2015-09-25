
namespace DungeonMasterEngine.Helpers
{
    public class BitMapMemory
    {
        private int xIndexOffset, yIndexOffset;
        private bool[,] bitMap;
        public BitMapMemory(int xIndexOffset, int yIndexOffset, int width, int height)
        {
            this.xIndexOffset = xIndexOffset;
            this.yIndexOffset = yIndexOffset;

            bitMap = new bool[width, height];
        }

        public bool this[int x, int y]
        {
            get { return bitMap[x - xIndexOffset, y - yIndexOffset]; }
            set { bitMap[x - xIndexOffset, y - yIndexOffset] = value; }
        }


    }
}
