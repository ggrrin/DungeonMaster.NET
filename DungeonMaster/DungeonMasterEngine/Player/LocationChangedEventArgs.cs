using System;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.Player
{
    public class LocationChangedEventArgs : EventArgs
    {
        public Tile NewLocation { get; }
        public Tile OldLocation { get; }

        public LocationChangedEventArgs(Tile oldLocation, Tile newLocation)
        {
            OldLocation = oldLocation;
            NewLocation = newLocation;
        }
    }
}