using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent
{

    public class TileNeighbours : INeighbours<ITile>
    {

        public TileNeighbours(Tile north, Tile south, Tile east, Tile west)
        {
            North = north;
            South = south;
            East = east;
            West = west;
        }

        public TileNeighbours(TileNeighbours neigbours)
        {
            North = neigbours.North;
            South = neigbours.South;
            East = neigbours.East;
            West = neigbours.West;
        }

        protected TileNeighbours()
        {
        }

        public virtual ITile North { get;  }
        public virtual ITile South { get; }
        public virtual ITile East { get; }
        public virtual ITile West { get; }

        public virtual ITile GetTile(MapDirection mapDirection)
        {
            if (mapDirection == MapDirection.East)
                return East;
            else if (mapDirection == MapDirection.West)
                return West;
            else if (mapDirection == MapDirection.North)
                return North;
            else if (mapDirection == MapDirection.South)
                return South;
            else
                return null;
        }

        public IEnumerator<Tuple<ITile, MapDirection>> GetEnumerator()
        {
            return MapDirection.AllDirections
                .Select(s => Tuple.Create(GetTile(s), s))
                .Where(t => t.Item1 != null)
                .GetEnumerator();
        }


        public override string ToString()
        {
            return $"North: {North} \r\n South: {South} \r\n East: {East} \r\n West: {West}";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}