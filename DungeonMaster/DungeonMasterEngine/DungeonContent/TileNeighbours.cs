using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent
{

    public class TileNeighbours : INeighbours<Tile>
    {
        public TileNeighbours()
        { }

        public TileNeighbours(TileNeighbours neigbours)
        {
            North = neigbours.North;
            South = neigbours.South;
            East = neigbours.East;
            West = neigbours.West;
        }

        public virtual Tile North { get; set; }
        public virtual Tile South { get; set; }
        public virtual Tile East { get; set; }
        public virtual Tile West { get; set; }

        public Tile GetTile(MapDirection mapDirection)
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

        public IEnumerator<Tuple<Tile, MapDirection>> GetEnumerator()
        {
            return MapDirection.AllSides
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