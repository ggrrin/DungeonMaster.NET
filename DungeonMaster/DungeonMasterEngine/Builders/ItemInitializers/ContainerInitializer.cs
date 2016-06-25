using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.GrabableItems.Initializers;

namespace DungeonMasterEngine.Builders.ItemInitializers
{
    class ContainerInitializer : IContainerInitializer
    {
        public ICollection<IGrabableItem> content { get; set; }
    }
}