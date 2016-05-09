using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Tiles;

namespace DungeonMasterEngine.Interfaces
{
    public interface INeighbours<TTile> : IEnumerable<Tuple<TTile,MapDirection>> where TTile : INeighbourable<TTile>
    {

        //TTile North { get; }

        //TTile South { get; }

        //TTile East { get; }

        //TTile West { get; }

        //TTile GetTile(Point shift);
    }
}