using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Constrains
{
    public class NoConstrain : IConstrain
    {
        public bool IsAcceptable(object item)
        {
            return true;
        }

        public override string ToString()
        {
            return "NoConstraion";
        }
    }
}