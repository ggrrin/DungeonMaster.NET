using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public interface IGrabableItem : ITextureRenderable, ILocalizable<ISpaceRouteElement>
    {
        IGrabableItemFactoryBase FactoryBase { get; }

        void SetLocationNoEvents(ISpaceRouteElement tile);
    }
}