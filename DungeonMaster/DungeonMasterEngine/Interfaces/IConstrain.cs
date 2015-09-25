using DungeonMasterEngine.Items;

namespace DungeonMasterEngine.Interfaces
{
    public interface IConstrain
    {
        bool IsAcceptable(GrabableItem item);

    }
}