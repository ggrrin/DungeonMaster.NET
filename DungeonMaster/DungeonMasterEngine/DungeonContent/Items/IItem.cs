using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public interface IItem : ILocalizable<Tile>
    {
        GrabableItem ExchangeItems(GrabableItem item);
        void Update(GameTime gameTime);
    }
}