using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport
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

        public FourthSpace GetSpace(Point gridPosition)
        {
            return allSpaces.First(x => x.GridPosition == gridPosition);
        }

        private IEnumerable<FourthSpace> allSpaces => fabric.Cast<FourthSpace>();
        public IEnumerable<ISpace> AllSpaces => allSpaces;

        public IEnumerable<ISpaceRouteElement> GetToSide(ISpaceRouteElement location, MapDirection mapDirection, bool useFullSpaces)
        {
            ISpace currentSpace = location.Space;// currentTile.LayoutManager.FindCurrentSpace(entity);
            ISpace destSpace = null;

            searcher.LayoutManager = useFullSpaces ? null : location.Tile.LayoutManager;
            searcher.StartSearch(AllSpaces.First(), currentSpace, 1, (space, layer, bundle) =>
            {
                if (space.Sides.Any(x => x == mapDirection))
                {
                    searcher.StopSearch();
                    destSpace = space;
                }
            });

            return destSpace != null ? searcher.GetShortestRoute(destSpace).Select(s => new FourthSpaceRouteElement(s, location.Tile)) : null;
        }
        private IEnumerable<ISpaceRouteElement> GetToSpace(ISpaceRouteElement location, ISpace destSpace, bool useFullSpaces)
        {
            bool found = false;
            ISpace currentSpace = location.Space; //currentTile.LayoutManager.FindCurrentSpace(entity);

            if (currentSpace == destSpace)
                return new FourthSpaceRouteElement(currentSpace, location.Tile).ToEnumerable();

            searcher.LayoutManager = useFullSpaces ? null : location.Tile.LayoutManager;
            searcher.StartSearch(AllSpaces.First(), currentSpace, 1, (space, layer, bundle) =>
            {
                if (space == destSpace)
                {
                    searcher.StopSearch();
                    found = true;
                }
            });

            return found ? searcher.GetShortestRoute(destSpace).Select(s => new FourthSpaceRouteElement(s, location.Tile)) : null;
        }

        public IEnumerable<ISpaceRouteElement> GetToNeighbour(ISpaceRouteElement location, ITile targetTile, bool useFullSpaces)
        {
            var moveDirection = location.Tile.Neighbors.Single(t => t.Item1 == targetTile).Item2;
            var currentSpace = location.Space; //currentTile.LayoutManager.FindCurrentSpace(entity);

            var curTileBridges = AllSpaces
                .Where(s => s.Sides.Contains(moveDirection))
                .Where(s => s == currentSpace || useFullSpaces || location.Tile.LayoutManager.IsFree(s));

            var targetTileBridges = AllSpaces
                .Where(s => s.Sides.Contains(moveDirection.Opposite))
                .Where(s => useFullSpaces || targetTile.LayoutManager.IsFree(s));

            var bridges = curTileBridges.Join(targetTileBridges,
                ospace => ospace.Sides.First(s => s != moveDirection),
                ispace => ispace.Sides.First(s => s != moveDirection.Opposite),
                Tuple.Create);

            var bridgesRoutes = bridges
                .Select(b => GetToSpace(location, b.Item1, useFullSpaces)?.Concat(new FourthSpaceRouteElement(b.Item2, targetTile).ToEnumerable()).ToArray())
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

        public ISpaceRouteElement GetSpaceElement(ISpace space, ITile tile)
        {
            return new FourthSpaceRouteElement(space, tile);
        }

        protected Small4GroupLayout()
        {
            for (int i = 0; i < fabric.GetLength(0); i++)
                for (int j = 0; j < fabric.GetLength(1); j++)
                    fabric[i, j].Neighbors = new FourthSpaceNeighbors(new Point(i, j), fabric);
        }

    }
}