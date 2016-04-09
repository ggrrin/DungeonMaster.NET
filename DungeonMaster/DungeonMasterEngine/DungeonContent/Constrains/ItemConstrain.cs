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
        public bool AcceptOthers { get; }

        public GrabableItemConstrain(int data, bool acceptOthers )
        {
            DataIndex = data;
            AcceptOthers = acceptOthers;
        }

        public bool IsAcceptable(object item)
        {
            var i = item as GrabableItem;
            return i?.Identifer == DataIndex ^ AcceptOthers;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {DataIndex}";
        }
    }
}