using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public interface IItem : ILocalizable<Tile> , IWorldObject
    {
        GrabableItem ExchangeItems(GrabableItem item);
        void Update(GameTime gameTime);
        BoundingBox Bounding { get; }
        bool AcceptMessages { get; set; }

    }
}