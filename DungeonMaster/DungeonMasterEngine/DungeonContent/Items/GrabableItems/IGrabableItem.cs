using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems
{
    public interface IGrabableItem : IItem 
    {
        IGrabableItemFactoryBase Factory { get; }
    }
}