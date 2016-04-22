using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Constrains
{
    public class GrabableItemConstrain : IConstrain
    {
        public int DataIndex { get; }

        /// <summary>
        /// Constraion accespts all grabable items with other DataIndex than specifed
        /// </summary>
        public bool InvertConstraion { get; }

        public GrabableItemConstrain(int data, bool invertConstraion )
        {
            DataIndex = data;
            InvertConstraion = invertConstraion;
        }

        public bool IsAcceptable(object item)
        {
            var i = item as GrabableItem;
            return i?.Identifer == DataIndex ^ InvertConstraion;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {DataIndex}";
        }
    }
}