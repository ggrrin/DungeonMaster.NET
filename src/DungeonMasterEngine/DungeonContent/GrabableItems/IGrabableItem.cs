using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.GrabableItems
{
    public interface IGrabableItem : ITextureRenderable, ILocalizable<ISpaceRouteElement>
    {
        IGrabableItemFactoryBase FactoryBase { get; }

        void SetLocationNoEvents(ISpaceRouteElement tile);
    }
}