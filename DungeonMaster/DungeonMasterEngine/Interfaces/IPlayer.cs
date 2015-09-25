using Microsoft.Xna.Framework;
using System;

namespace DungeonMasterEngine.Interfaces
{
    public interface IPlayer : ILocalizable<Tile>
    {
        event EventHandler LocationChanged;
    }
}