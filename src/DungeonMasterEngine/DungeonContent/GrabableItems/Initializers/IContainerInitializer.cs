using System.Collections.Generic;

namespace DungeonMasterEngine.DungeonContent.GrabableItems.Initializers
{
    public interface IContainerInitializer {
        ICollection<IGrabableItem> content { get; }
    }
}