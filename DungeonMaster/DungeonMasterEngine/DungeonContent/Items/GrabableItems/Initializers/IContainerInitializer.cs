using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers
{
    public interface IContainerInitializer {
        ICollection<IGrabableItem> content { get; }
    }
}