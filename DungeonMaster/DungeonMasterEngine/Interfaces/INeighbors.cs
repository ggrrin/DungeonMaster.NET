using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.Interfaces
{
    public interface INeighbors<TTile> : IEnumerable<Tuple<TTile,MapDirection>> where TTile : INeighbourable<TTile>
    {

        //TTile North { get; }

        //TTile South { get; }

        //TTile East { get; }

        //TTile West { get; }

        //TTile GetTile(Point shift);
    }
}