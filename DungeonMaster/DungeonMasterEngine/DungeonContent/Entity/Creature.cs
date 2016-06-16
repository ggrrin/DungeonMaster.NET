using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.@base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.@base;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public class Creature : LiveEntity
    {
        private static readonly Random random = new Random();
        private static readonly BreadthFirstSearch<ITile, object> globalSearcher = new BreadthFirstSearch<ITile, object>();
        private readonly Animator<Creature, ISpaceRouteElement> animator = new Animator<Creature, ISpaceRouteElement>();
        private readonly BreadthFirstSearch<ITile, uint> watchAroundArea = new BreadthFirstSearch<ITile, uint>();
        private ITile watchAroungOrigin;

        public override IRelationManager RelationManager { get; }
        public override IBody Body { get; }

        private IProperty[] properties = new IProperty[]
        {

        };

        public override IProperty GetProperty(IPropertyFactory propertyType) => properties.FirstOrDefault(p => p.Type == propertyType) ?? new EmptyProperty();

        public override ISkill GetSkill(ISkillFactory skillType)
        {
            throw new NotImplementedException();
        }

        public int DetectRange { get; }
        public int SightRange { get; }
        public bool hounting => hountingPath != null;

        public int MoveDuration
        {
            get { return (int)(moveDuration * random.Next(9, 11) / 10f); }
            protected set { moveDuration = value; }
        }

        public override float TranslationVelocity => 4;
        public int watchAroundRadius { get; protected set; } = 3;
        public override IGroupLayout GroupLayout { get; }

        private ISpaceRouteElement location = null;
        public override ISpaceRouteElement Location
        {
            get { return location; }
            set
            {
                if (location == value)
                    throw new InvalidOperationException("Cannot move from space to itself.");

                bool alreadyOnTile = location.Tile == value.Tile;

                if(!alreadyOnTile)
                    location?.Tile?.OnObjectLeft(this);

                location = value;

                if(!alreadyOnTile)
                    location?.Tile?.OnObjectEntered(this);
            }
        }

        private bool living = false;
        private int moveDuration = 2000;
        private int attackDuration = 1000;
        private IReadOnlyList<ITile> hountingPath = null;
        private bool gettingHome => homeRoute != null;
        private IReadOnlyList<ITile> homeRoute = null;

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

        public Creature(IGroupLayout layout, ISpaceRouteElement location, RelationToken relationToken, IEnumerable<RelationToken> enemiesTokens, int moveDuration, int detectRange, int sightRange) 
        {
            GroupLayout = layout;
            MoveDuration = moveDuration;
            DetectRange = detectRange;
            SightRange = sightRange;
            watchAroungOrigin = location.Tile;

            if (!location.Tile.LayoutManager.TryGetSpace(this, location.Space))
                throw new ArgumentException("Location is not accessable!");

            ((ILocalizable<ISpaceRouteElement>)this).Location = location;
            RelationManager = new DefaultRelationManager(relationToken, enemiesTokens);
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

            Location.Tile.LayoutManager.FreeSpace(this, location.Space);
            //TODO
            //((CubeGraphic) GraphicsProvider).Texture = ResourceProvider.Instance.DrawRenderTarget("DEAD", Color.Black, Color.Red);
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
                if (!await MoveToNeighbourTile(tile, findEnemies: true))
                    break;
            }
        }

        private async Task<bool> MoveToNeighbourTile(ITile tile, bool findEnemies)
        {
            if (tile.IsAccessible) //TODO other conditions
            {
                if (await MoveThroughSpaces(tile, findEnemies))
                {
                    if (!hounting && !gettingHome)
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

        private async Task<bool> MoveThroughSpaces(ITile targetTile, bool findEnemies)
        {
            var spaceRoute = GroupLayout.GetToNeighbour(this, Location.Tile, targetTile);

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
            if (destination.Tile.LayoutManager.TryGetSpace(this, destination.Space))
            {
                //free previous location
                location?.Tile.LayoutManager.FreeSpace(this, location.Space);
                await animator.MoveToAsync(this, destination, setLocation: true);
                return true;
            }
            else
                return false;
        }

        private IReadOnlyList<ITile> FindNextWatchLocation()
        {
            var maxTravelDistance = random.Next(2, 2 * watchAroundRadius + 1);
            ITile destTile = Location.Tile;
            uint destTileUsages = 0;
            int desDist = 0;
            watchAroundArea.StartSearch(watchAroungOrigin, Location.Tile, watchAroundRadius, (tile, distance, bundle) =>
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
            ILiveEntity enemy = null;
            globalSearcher.StartSearch(Location.Tile, Location.Tile, Math.Max(DetectRange, SightRange), (tile, layer, bundle) =>
            {
                enemy = tile.LayoutManager.Entities.FirstOrDefault(e => RelationManager.IsEnemy(e.RelationManager.RelationToken));
                if (enemy != null)
                    globalSearcher.StopSearch();
            });
            if (enemy != null)
            {
                hountingPath = globalSearcher.GetShortestRoute(enemy.Location.Tile);
                $"{this} found enemies at {hountingPath.Last().GridPosition}".Dump();
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
                    await PrepareForFight(hountingPath[1]);
                }
                else
                {
                    await MoveToNeighbourTile(hountingPath.Skip(1).First(), findEnemies: false);
                }

                if (!FindEnemies())
                {
                    if (!GetPathHome())
                        EstablishNewBase();
                }
            }
        }

        private bool GetPathHome()
        {
            var distance = watchAroungOrigin.GridPosition - Location.Tile.GridPosition;//TODO calculate appropriate distance
            var maxDistance = Math.Max(Math.Abs(distance.X), Math.Abs(distance.Y));
            ITile destTile = null;
            globalSearcher.StartSearch(Location.Tile, Location.Tile, maxDistance, (tile, layer, bundle) =>
            {
                if (tile == watchAroungOrigin)
                {
                    globalSearcher.StopSearch();
                    destTile = tile;
                }
            });
            if (destTile == null) //lost
            {
                $"{this} lost".Dump();
                homeRoute = null;
                return false;
            }
            else
            {
                $"{this} going home.".Dump();
                homeRoute = globalSearcher.GetShortestRoute(destTile);
                return true;
            }
        }

        private void EstablishNewBase()
        {

            $"{this} reestablishing base at {Location}.".Dump();
            watchAroundArea.ClearBundles();
            watchAroungOrigin = Location.Tile;
        }

        private async Task PrepareForFight(ITile enemyTile)
        {
            var moveDirection = Location.Tile.Neighbours.Single(t => t.Item1 == enemyTile).Item2;

            while (true)
            {
                var routeToSide = GroupLayout.GetToSide(this, Location.Tile, moveDirection);
                if (routeToSide != null)
                {
                    foreach (var space in routeToSide.Skip(1))
                    {
                        if (!await MoveToSpace(space))
                        {
                            goto whileLoop;
                        }
                        await Task.Delay(MoveDuration);
                    }

                    await Fight(enemyTile, moveDirection);
                    break;
                }

                whileLoop:
                await Task.Delay(100);
            }
        }

        private async Task Fight(ITile enemyTile, MapDirection moveDirection)
        {
            var sortedEnemyLocation = GroupLayout.AllSpaces
                .Where(s => s.Sides.Contains(moveDirection.Opposite))
                .Concat(GroupLayout.AllSpaces
                    .Where(s => s.Sides.Contains(moveDirection)))
                .Where(s => !enemyTile.LayoutManager.IsFree(s));

            var locEnum = sortedEnemyLocation.GetEnumerator();


            while (locEnum.MoveNext() && living)
            {
                await Task.Delay(100);

                if (enemyTile.LayoutManager.WholeTileEmpty)
                    break;

                ILiveEntity enemy;
                do
                {
                    enemy = enemyTile.LayoutManager.GetEntities(locEnum.Current).FirstOrDefault();
                    if (enemy != null)
                    {
                        $"{enemy} hitted.".Dump();
                        await Task.Delay(attackDuration);
                        //TODO if killed reset enumerator
                        if (false)
                        {
                            locEnum.Reset();
                            break;
                        }
                    }
                }
                while (living && enemy != null);
            }
        }

        public override void MoveTo(ITile newLocation, bool setNewLocation)
        {

        }

        public override void Update(GameTime time)
        {
            animator.Update(time);
        }

        public override string ToString()
        {
            return "creature";
        }

        public void Kill()
        {
            living = false;
        }
    }
}