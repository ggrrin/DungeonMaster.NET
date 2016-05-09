using System;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.Interfaces
{
    public interface IPlayer : ILocalizable<Tile>
    {
        event EventHandler LocationChanged;
    }
}