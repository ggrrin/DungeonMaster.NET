using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport
{
    class FourthSpaceNeighbors : INeighbors<ISpace>
    {
        private readonly IEnumerable<Tuple<ISpace, MapDirection>> neighbours;

        public FourthSpaceNeighbors(Point gridPosition, FourthSpace[,] fabric)
        {
            neighbours = MapDirection.Sides
                .Select(side => Tuple.Create<MapDirection, Point>(side, gridPosition + side))
                .Where(p => p.Item2.X >= 0 && p.Item2.Y >= 0)
                .Where(p => p.Item2.X < fabric.GetLength(0) && p.Item2.Y < fabric.GetLength(1))
                .Select(p => Tuple.Create((ISpace)fabric[p.Item2.X, p.Item2.Y], p.Item1))
                .ToArray();
        }

        private FourthSpaceNeighbors()
        {
            neighbours = Enumerable.Empty<Tuple<ISpace, MapDirection>>();
        }

        public static FourthSpaceNeighbors Empty { get; } = new FourthSpaceNeighbors();

        public IEnumerator<Tuple<ISpace, MapDirection>> GetEnumerator() => neighbours.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}