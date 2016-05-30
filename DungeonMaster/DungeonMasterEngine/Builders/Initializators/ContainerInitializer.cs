using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Initializers;

namespace DungeonMasterEngine.Builders.Initializators
{
    class ContainerInitializer : IContainerInitializer
    {
        public ICollection<IGrabableItem> content { get; set; }
    }
}