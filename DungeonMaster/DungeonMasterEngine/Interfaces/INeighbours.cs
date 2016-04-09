using Microsoft.Xna.Framework;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.Interfaces
{
    public interface INeighbours : IEnumerable<KeyValuePair<Tile,Point>>
    {

        Tile North { get; }

        Tile South { get; }

        Tile East { get; }

        Tile West { get; }

        Tile GetTile(Point shift);
    }
}