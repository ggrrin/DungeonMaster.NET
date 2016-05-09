using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class Small4GroupLayout : IGroupLayout
    {
        public static Small4GroupLayout Instance { get; } = new Small4GroupLayout();

        private readonly GroupLayoutSearcher searcher = new GroupLayoutSearcher();

        private readonly FourthSpace[,] fabric = {
            {
                new FourthSpace(0, 0, new [] {MapDirection.West,MapDirection.North}),
                new FourthSpace(1, 0, new [] {MapDirection.North,MapDirection.East})},
            {
                new FourthSpace(0, 1, new [] {MapDirection.West, MapDirection.South}),
                new FourthSpace(1, 1, new [] {MapDirection.South, MapDirection.East})
            },
        };

        public IEnumerable<ISpace> AllSpaces => fabric.Cast<FourthSpace>();

        public IEnumerable<ISpaceRouteElement> GetToSide(ILayoutable entity, Tile currentTile, MapDirection mapDirection)
        {
            //TODO remake
            ISpace currentSpace = currentTile.LayoutManager.FindCurrentSpace(entity);
            ISpace destSpace = null;

            searcher.LayoutManager = currentTile.LayoutManager;
            searcher.StartSearch(AllSpaces.First(), currentSpace, 1, (space, layer, bundle) =>
            {
                if (space.Sides.Any(x => x == mapDirection))
                {
                    searcher.StopSearch();
                    destSpace = space;
                }
            });

            Debug.Assert(destSpace != null);
            return searcher.GetShortestRoute(destSpace).Select(s => new FourthSpaceRouteElement(s, currentTile));
        }
        private IEnumerable<ISpaceRouteElement> GetToSpace(ILayoutable entity, Tile currentTile, ISpace destSpace)
        {
            bool found = false;
            ISpace currentSpace = currentTile.LayoutManager.FindCurrentSpace(entity);

            if (currentSpace == destSpace)
                return new FourthSpaceRouteElement(currentSpace, currentTile).ToEnumerable();

            searcher.LayoutManager = currentTile.LayoutManager;
            searcher.StartSearch(AllSpaces.First(), currentSpace, 1, (space, layer, bundle) =>
            {
                if (space == destSpace)
                {
                    searcher.StopSearch();
                    found = true;
                }
            });

            return found ? searcher.GetShortestRoute(destSpace).Select(s => new FourthSpaceRouteElement(s, currentTile)) : null;
        }

        public IEnumerable<ISpaceRouteElement> GetToNeighbour(ILayoutable entity, Tile currentTile, Tile targetTile)
        {
            var moveDirection = currentTile.Neighbours.Single(t => t.Item1 == targetTile).Item2;
            var currentSpace = currentTile.LayoutManager.FindCurrentSpace(entity);

            var curTileBridges = entity.GroupLayout.AllSpaces
                .Where(s => s.Sides.Contains(moveDirection))
                .Where(s => s == currentSpace || currentTile.LayoutManager.IsFree(s));

            var targetTileBridges = entity.GroupLayout.AllSpaces
                .Where(s => s.Sides.Contains(moveDirection.Opposite))
                .Where(s => targetTile.LayoutManager.IsFree(s));

            var bridges = curTileBridges.Join(targetTileBridges,
                ospace => ospace.Sides.First(s => s != moveDirection),
                ispace => ispace.Sides.First(s => s != moveDirection.Opposite),
                Tuple.Create);

            var bridgesRoutes = bridges
                .Select(b => GetToSpace(entity, currentTile, b.Item1)?.Concat(new FourthSpaceRouteElement(b.Item2, targetTile).ToEnumerable()).ToArray())
                .Where(r => r != null)
                .ToArray();

            if (bridgesRoutes.Any())
            {
                var minLen = bridgesRoutes.Min(x => x.Length);
                return bridgesRoutes.First(br => br.Length == minLen);
            }
            else
                return null;
        }


        private Small4GroupLayout()
        {
            for (int i = 0; i < fabric.GetLength(0); i++)
                for (int j = 0; j < fabric.GetLength(1); j++)
                    fabric[i, j].Neighbours = new N4(new Point(i, j), fabric);
        }

    }
}