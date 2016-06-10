using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    class N4 : INeighbours<ISpace>
    {
        private readonly IEnumerable<Tuple<ISpace, MapDirection>> neighbours;

        public N4(Point gridPosition, FourthSpace[,] fabric)
        {
            neighbours = MapDirection.Sides
                .Select(side => Tuple.Create<MapDirection, Point>(side, gridPosition + side))
                .Where(p => p.Item2.X >= 0 && p.Item2.Y >= 0)
                .Where(p => p.Item2.X < fabric.GetLength(0) && p.Item2.Y < fabric.GetLength(1))
                .Select(p => Tuple.Create((ISpace)fabric[p.Item2.X, p.Item2.Y], p.Item1))
                .ToArray();
        }

        private N4()
        {
            neighbours = Enumerable.Empty<Tuple<ISpace, MapDirection>>();
        }

        public static N4 Empty { get; } = new N4();

        public IEnumerator<Tuple<ISpace, MapDirection>> GetEnumerator() => neighbours.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}