using System;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.Player
{
    public class LocationChangedEventArgs : EventArgs
    {
        public ITile NewLocation { get; }
        public ITile OldLocation { get; }

        public LocationChangedEventArgs(ITile oldLocation, ITile newLocation)
        {
            OldLocation = oldLocation;
            NewLocation = newLocation;
        }
    }
}