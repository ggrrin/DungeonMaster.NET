using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using DungeonMasterParser;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public class Creature : LivingItem, ILayoutable
    {
        private static readonly Random random = new Random();
        private static readonly BreadthFirstSearch<Tile, object> globalSearcher = new BreadthFirstSearch<Tile, object>();
        private readonly Animator<Creature, ISpaceRouteElement> animator = new Animator<Creature, ISpaceRouteElement>();
        private readonly BreadthFirstSearch<Tile, uint> watchAroundArea = new BreadthFirstSearch<Tile, uint>();
        private readonly Tile initialTile;

        public override IRelationManager RelationManager { get; }
        public int DetectRange { get; }
        public int SightRange { get; }
        public bool hounting => hountingPath != null;

        public int MoveDuration
        {
            get { return (int)(moveDuration * random.Next(9, 11) / 10f); }
            protected set { moveDuration = value; }
        }

        public float TranslationVelocity { get; } = 4;
        public int watchAroundRadius { get; protected set; } = 3;
        public IGroupLayout GroupLayout { get; }

        private ISpaceRouteElement location = null;
        ISpaceRouteElement ILocalizable<ISpaceRouteElement>.Location
        {
            get { return location; }
            set
            {
                if (location == value)
                    throw new InvalidOperationException("Cannot move from space to itself.");

                location = value;

                if (Location != location.SpaceParent)
                    Location = location.SpaceParent;
            }
        }

        private bool living = false;
        private int moveDuration = 2000;
        private IReadOnlyList<Tile> hountingPath = null;
        private bool gettingHome => homeRoute != null;
        private IReadOnlyList<Tile> homeRoute = null;

        public bool Living
        {
            get { return living; }
            set
            {
                living = value;
                if (living = value)
                    Live();
            }
        }

        public Creature(IGroupLayout layout, ISpaceRouteElement location, RelationToken relationToken, IEnumerable<RelationToken> enemiesTokens, int moveDuration, int detectRange, int sightRange) : base(location.StayPoint)
        {
            GroupLayout = layout;
            MoveDuration = moveDuration;
            DetectRange = detectRange;
            SightRange = sightRange;
            initialTile = location.SpaceParent;

            if (!location.SpaceParent.LayoutManager.TryGetSpace(this, location.Space))
                throw new ArgumentException("Location is not accessable!");

            ((ILocalizable<ISpaceRouteElement>)this).Location = location;
            RelationManager = new DefaultRelationManager(relationToken, enemiesTokens);
        }

        public override GrabableItem ExchangeItems(GrabableItem item)
        {
            living ^= true;
            if (Living)
                Live();

            return base.ExchangeItems(item);
        }

        private async void Live()
        {
            while (living)
            {
                if (hounting)
                    await Hount();
                else if (gettingHome)
                    await GoHome();
                else
                    await WatchAround();

                await Task.Delay(100);
            }
        }

        private async Task GoHome()
        {
            foreach (var tile in homeRoute.Skip(1))//first tile is current tile
            {
                if (!await MoveToNeighbourTile(tile, findEnemies: true))
                    break;
            }

            homeRoute = null;
        }

        private async Task WatchAround()
        {
            var nextRoute = FindNextWatchLocation();
            foreach (var tile in nextRoute.Skip(1))//first tile is current tile
            {
                if (!await MoveToNeighbourTile(tile, findEnemies:true))
                    break;
            }
        }

        private async Task<bool> MoveToNeighbourTile(Tile tile, bool findEnemies)
        {
            if (tile.IsAccessible) //TODO other conditions
            {
                if (await MoveThroughSpaces(tile, findEnemies))
                {
                    if(!hounting && !gettingHome)
                        watchAroundArea.SetBundle(tile, watchAroundArea.GetBundle(tile) + 1);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private async Task<bool> MoveThroughSpaces(Tile tile, bool findEnemies )
        {
            var spaceRoute = GroupLayout.GetToNeighbour(this, Location, tile);

            if (spaceRoute == null)
                return false;

            foreach (var space in spaceRoute.Skip(1))
            {
                if (findEnemies && FindEnemies())
                    return false;

                var duration = Task.Delay(MoveDuration);
                if (!await MoveToSpace(space))
                    return false;

                await duration;
            }
            return true;
        }

        private async Task<bool> MoveToSpace(ISpaceRouteElement destination)
        {
            if (destination.SpaceParent.LayoutManager.TryGetSpace(this, destination.Space))
            {
                //free previous location
                location?.SpaceParent.LayoutManager.FreeSpace(this, location.Space);
                await animator.MoveToAsync(this, destination);
                return true;
            }
            else
                return false;
        }

        private IReadOnlyList<Tile> FindNextWatchLocation()
        {
            var maxTravelDistance = random.Next(2, 2 * watchAroundRadius + 1);
            Tile destTile = Location;
            uint destTileUsages = 0;
            int desDist = 0;
            watchAroundArea.StartSearch(initialTile, Location, watchAroundRadius, (tile, distance, bundle) =>
            {
                if (tile == null)
                    throw new Exception();

                if (distance > maxTravelDistance)
                    watchAroundArea.StopSearch();

                if ((desDist < distance) || (desDist == distance && destTileUsages < bundle))
                {
                    destTile = tile;
                    destTileUsages = bundle;
                    desDist = distance;
                }
            });

            if (destTile == null)
                throw new Exception();

            return watchAroundArea.GetShortestRoute(destTile);
        }

        private bool FindEnemies()
        {
            ILayoutable enemy = null;
            globalSearcher.StartSearch(Location, Location, Math.Max(DetectRange, SightRange), (tile, layer, bundle) =>
            {
                enemy = tile.LayoutManager.Entities.FirstOrDefault(e => RelationManager.IsEnemy(e.RelationManager.RelationToken));
                if (enemy != null)
                    globalSearcher.StopSearch();
            });
            if (enemy != null)
            {
                hountingPath = globalSearcher.GetShortestRoute(enemy.Location.SpaceParent);
                return true;
            }
            else
            {
                hountingPath = null;
                return false;
            }
        }

        private async Task Hount()
        {
            if (hountingPath != null)
            {
                if (hountingPath.Count == 2)
                {
                    Fight();
                }
                else
                {

                    await MoveToNeighbourTile(hountingPath.Skip(1).First(), findEnemies:false);

                    if (!FindEnemies())
                    {
                        if (!GetPathHome())
                            EstablishNewBase();
                    }
                }
            }
        }

        private bool GetPathHome()
        {
            var distance  = initialTile.GridPosition - Location.GridPosition;//TODO calculate appropriate distance
            var maxDistance = Math.Max(Math.Abs(distance.X), Math.Abs(distance.Y));
            Tile destTile = null;
            globalSearcher.StartSearch(Location, Location, maxDistance, (tile, layer, bundle) =>
            {
                if (tile == initialTile)
                {
                    globalSearcher.StopSearch();
                    destTile = tile;
                }
            });
            if (destTile == null) //lost
            {
                homeRoute = null;
                return false;
            }
            else
            {
                homeRoute = globalSearcher.GetShortestRoute(destTile);
                return true;
            }
        }

        private void EstablishNewBase()
        {
            throw new NotImplementedException();
        }

        private void Fight()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            if (animator.IsAnimating)
                Position += animator.GetTranslation(time);
        }

        protected override void OnLocationChanged()
        {
            //$"{Location.Position}".Dump();            
        }

    }
}