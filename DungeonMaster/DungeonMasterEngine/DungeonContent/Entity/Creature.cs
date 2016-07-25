using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.Builders.CreatureCreators;
using DungeonMasterEngine.DungeonContent.Actions;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties.CreatureSpecific;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity
{
    public class Creature : LiveEntity, IRenderable
    {
        public CreatureFactory Factory { get; }
        private static readonly Random random = new Random();
        private static readonly BreadthFirstSearch<ITile, object> globalSearcher = new BreadthFirstSearch<ITile, object>();
        private readonly Animator<Creature, ISpaceRouteElement> animator = new Animator<Creature, ISpaceRouteElement>();
        private readonly BreadthFirstSearch<ITile, uint> watchAroundArea = new BreadthFirstSearch<ITile, uint>();
        private ITile watchAroungOrigin;

        public override IRelationManager RelationManager { get; }
        public override IBody Body { get; }

        private readonly Dictionary<IPropertyFactory, IProperty> properties;

        public IRenderer Renderer { get; set; }

        public int DetectRange => Factory.DetectRange;

        public int SightRange => Factory.SightRange;

        public bool hounting => hountingPath != null;

        public int MoveDuration => (int)(Factory.MoveDuration * random.Next(9, 11) / 10f);

        public override float TranslationVelocity => 4;

        public int watchAroundRadius { get; protected set; } = 3;

        public override IGroupLayout GroupLayout => Factory.Layout;

        protected ISpaceRouteElement location = null;

        protected int attackDuration => Factory.AttackTicks;

        protected IReadOnlyList<ITile> hountingPath = null;

        protected bool gettingHome => homeRoute != null;

        protected IReadOnlyList<ITile> homeRoute = null;

        public override ISpaceRouteElement Location
        {
            get { return location; }
            set
            {
                if (location == value)
                    throw new InvalidOperationException("Cannot move from space to itself.");

                bool alreadyOnTile = location?.Tile == value?.Tile;
                bool differentLevel = location?.Tile?.Level != value?.Tile?.Level;

                if (animator.IsAnimating)
                    animator.AbortFinishAsync();

                if (value != null && !value.Tile.LayoutManager.TryGetSpace(this, value.Space))
                    throw new ArgumentException($"Location is not accessible, check it before assigning to {nameof(Location)}!");

                if (!alreadyOnTile)
                {
                    location?.Tile?.OnObjectLeft(this);

                    if (differentLevel)
                    {
                        location?.Tile?.Level?.Updateables.Remove(this);
                    }
                }

                if(value != null)
                    Position = value.StayPoint;

                var prevLocation = location;
                location = value;

                if (!alreadyOnTile)
                {
                    location?.Tile?.OnObjectEntered(this);

                    if (differentLevel)
                    {
                        location?.Tile?.Level?.Updateables.Add(this);
                    }
                }
                prevLocation?.Tile.LayoutManager.FreeSpace(this, prevLocation.Space);
            }
        }

        public IReadOnlyList<IGrabableItem> Possessions { get; }


        private static int j = 0;
        public int ID { get; }

        public Creature(ICreatureInitializer initializer, CreatureFactory factory)
        {
            ID = j++;
            Factory = factory;
            RelationManager = new DefaultRelationManager(initializer.RelationToken, initializer.EnemiesTokens);
            Possessions = initializer.PossessionItems.ToList();

            var loc = initializer.Location;
            if (!initializer.Location.Tile.IsInitialized)
                initializer.Location.Tile.Initialized += (s, e) => InitializeTileRelated(loc);
            else
                InitializeTileRelated(initializer.Location);

            //if (!initializer.Location.Tile.LayoutManager.TryGetSpace(this, initializer.Location.Space))
            //    throw new ArgumentException("Location is not accessible!");

            HealthProperty health;
            properties = InitProperties(initializer.HitPoints, out health);
            health.ValueChanged += TryDie;
        }

        private Dictionary<IPropertyFactory, IProperty> InitProperties(int hitPoints, out HealthProperty health)
        {
            return new Dictionary<IPropertyFactory, IProperty>
            {
                {PropertyFactory<HealthProperty>.Instance, health = new HealthProperty(hitPoints) },
                {PropertyFactory<ExperienceProperty>.Instance, new ExperienceProperty(Factory.Experience)},
                {PropertyFactory<DefenseProperty>.Instance, new DefenseProperty(Factory.Defense)},
                {PropertyFactory<DextrityProperty>.Instance, new DextrityProperty(Factory.Dexterity)},
                {PropertyFactory<AntiFireProperty>.Instance, new DextrityProperty(Factory.FireResistance)},
                {PropertyFactory<AntiPoisonProperty>.Instance, new DextrityProperty(Factory.PoisonResistance)},
            };
        }

        private void TryDie(object sender, int value)
        {
            if (Activated && value <= 0)
            {
                Activated = false;

                //if (animator.IsAnimating && animator.AnimatingTask != null)
                //    await animator.AnimatingTask;

                //location.Tile.LayoutManager.FreeSpace(this, location.Space);
                //location.Tile.OnObjectLeft(this);
                //location.Tile.Level.Updateables.Remove(this);

                foreach (var item in Possessions)
                {
                    item.Location = GroupLayout.GetSpaceElement(location.Space, location.Tile);
                }
                Location = null;
            }
        }

        protected void InitializeTileRelated(ISpaceRouteElement element)
        {
            Location = element;

            watchAroungOrigin = element.Tile;
            LiveAsync();
        }

        public override IEnumerable<ISkill> Skills => Enumerable.Empty<ISkill>();

        public override IProperty GetProperty(IPropertyFactory propertyType)
        {
            IProperty res;
            properties.TryGetValue(propertyType, out res);
            return res ?? new EmptyProperty();
        }

        public override IEnumerable<IProperty> Properties => properties.Values;

        public override ISkill GetSkill(ISkillFactory skillType)
        {
            return new EmptySkill();
        }

        public override async void MoveTo(ISpaceRouteElement newLocation)
        {
            await MoveToAsync(newLocation);
        }

        public override async Task<bool> MoveToAsync(ISpaceRouteElement newLocation)
        {
            if (location == newLocation)
                throw new InvalidOperationException("Cannot move from space to itself.");

            bool alreadyOnTile = location?.Tile == newLocation.Tile;
            bool differentLevel = location?.Tile?.Level != newLocation?.Tile?.Level;

            if (animator.IsAnimating)
                animator.AbortFinishAsync();

            if (!newLocation.Tile.LayoutManager.TryGetSpace(this, newLocation.Space))
                return false;


            //following line is done by animator itself
            //Position = newLocation.StayPoint;

            location?.Tile.LayoutManager.FreeSpace(this, location.Space);
            await animator.MoveToAsync(this, newLocation, false);


            if (!alreadyOnTile)
            {
                location?.Tile?.OnObjectLeft(this);

                if (differentLevel)
                {
                    location?.Tile?.Level?.Updateables.Remove(this);
                }
            }

            location = newLocation;

            if (!alreadyOnTile)
            {
                location?.Tile?.OnObjectEntered(this);

                if (differentLevel)
                {
                    location?.Tile?.Level?.Updateables.Add(this);
                }
            }
            //move following line before animation start in order to let creatures
            //move more consistently
            //location?.Tile.LayoutManager.FreeSpace(this, location.Space);
            return true;
        }

        protected virtual async void LiveAsync()
        {
            Activated = true;
            //function is not split on every await, 
            //instead only if some "bigger operation" occurred
            //such as Task.Yield(), Task.Delay(), awaiting
            //for TaskCompletionSource etc.
            //Thus here we want to return from async call
            //to let creature move after whole initialization
            await Task.Yield();
            while (Activated)
            {
                if (hounting)
                    await Hount();
                else if (gettingHome)
                    await GoHome();
                else
                    await WatchAround();

                await Task.Delay(100);
                //wait for finish move by MoveToAsync method (called for example by pit)
                if (animator.AnimatingTask != null)
                    await animator.AnimatingTask;
            }
        }

        protected virtual async Task GoHome()
        {
            foreach (var tile in homeRoute.Skip(1))//first tile is current tile
            {
                if (!await MoveToNeighbourTile(tile, findEnemies: true))
                    break;
            }

            homeRoute = null;
        }

        protected virtual async Task WatchAround()
        {
            if (Location == null)
                return;

            var nextRoute = FindNextWatchLocation();
            foreach (var tile in nextRoute.Skip(1))//first tile is current tile
            {
                if (!await MoveToNeighbourTile(tile, findEnemies: true))
                    break;
            }
        }

        protected virtual async Task<bool> MoveToNeighbourTile(ITile tile, bool findEnemies)
        {
            if (tile.IsAccessible && !tile.IsDangerous)
            {
                //move was successful and no teleporation occurred afterwards
                if (await MoveThroughSpaces(tile, findEnemies) && Location == tile)
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

        protected virtual async Task<bool> MoveThroughSpaces(ITile targetTile, bool findEnemies)
        {
            if (Location == null)
                return false;

            var spaceRoute = GroupLayout.GetToNeighbour(Location, targetTile, false);

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

        protected virtual async Task<bool> MoveToSpace(ISpaceRouteElement destination)
        {
            //something stole control to creature, i.e. falling to pit
            if (animator.IsAnimating)
            {
                //wait until finish move
                await animator.AnimatingTask;
                //reset movement
                return false;
            }

            bool EnemyAtTile = destination.Tile.LayoutManager.Entities.Any(x => RelationManager.IsEnemy(x.RelationManager.RelationToken));
            if (Activated && destination.Tile.IsAccessible && !EnemyAtTile)
            {
                return await MoveToAsync(destination);
            }
            else
                return false;
        }

        protected virtual IReadOnlyList<ITile> FindNextWatchLocation()
        {
            if (Location == null)
                return Enumerable.Empty<ITile>().ToList();

            var maxTravelDistance = random.Next(2, 2 * watchAroundRadius + 1);
            ITile destTile = Location.Tile;
            uint destTileUsages = 0;
            int desDist = 0;

            //TODO current location is outside of range ?? shouldnt by solved by go home rutine ? 
            int distanceFromOrigin = (int)(watchAroungOrigin.GridPosition - Location.Tile.GridPosition).ToVector2().Length();
            watchAroundArea.StartSearch(watchAroungOrigin, Location.Tile, Math.Max(distanceFromOrigin, watchAroundRadius), (tile, distance, bundle) =>
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

        protected virtual bool FindEnemies()
        {
            if (Location == null)
            {
                hountingPath = null;
                return false;
            }

            ILiveEntity enemy = null;
            ITile matchedTile = null;
            globalSearcher.StartSearch(Location.Tile, Location.Tile, Math.Max(DetectRange, SightRange), (tile, layer, bundle) =>
            {
                enemy = tile.LayoutManager.Entities.FirstOrDefault(e => RelationManager.IsEnemy(e.RelationManager.RelationToken));
                if (enemy != null)
                {
                    globalSearcher.StopSearch();
                    matchedTile = tile;
                }

            });
            if (enemy != null)
            {
                hountingPath = globalSearcher.GetShortestRoute(matchedTile);
                //$"{this} found enemies at {hountingPath.Last().GridPosition}".Dump();
                return true;
            }
            else
            {
                hountingPath = null;
                return false;
            }
        }

        protected virtual async Task Hount()
        {
            if (Location == null)
                return;

            if (hountingPath != null)
            {
                if (hountingPath.Count == 2)
                {
                    await PrepareForFight(hountingPath[1]);
                }
                else
                {
                    await MoveToNeighbourTile(hountingPath.Skip(1).First(), findEnemies: false);
                    //TODO exception when on the same tile as enemy
                }

                if (!FindEnemies())
                {
                    if (!GetPathHome())
                        EstablishNewBase();
                }
            }
        }

        protected virtual bool GetPathHome()
        {
            if (Location == null)
                return false;

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
                //$"{this} lost".Dump();
                homeRoute = null;
                return false;
            }
            else
            {
                //$"{this} going home.".Dump();
                homeRoute = globalSearcher.GetShortestRoute(destTile);
                return true;
            }
        }

        protected virtual void EstablishNewBase()
        {
            if (Location == null)
                return;

            //$"{this} reestablishing base at {Location}.".Dump();
            watchAroundArea.ClearBundles();
            watchAroungOrigin = Location.Tile;
        }

        protected virtual async Task PrepareForFight(ITile enemyTile)
        {
            if (Location == null)
                return;

            var moveDirection = Location.Tile.Neighbors.SingleOrDefault(t => t.Item1 == enemyTile)?.Item2;
            if (moveDirection == null)
            {
                //creature fall to pit for example
                return;
            }

            var routeToSide = GroupLayout.GetToSide(Location, moveDirection.Value, false);
            if (routeToSide != null)
            {
                foreach (var space in routeToSide.Skip(1))
                {
                    if (!await MoveToSpace(space))
                    {
                        return;
                    }
                    await Task.Delay(MoveDuration);
                }
                if (Location == null)
                    return;
                new CreatureAttack(this).Apply(moveDirection.Value);
                await Task.Delay(attackDuration);
            }
        }

        public override void Update(GameTime time)
        {
            animator.Update(time);
        }

        public override string ToString()
        {
            return $"creature {ID}";
        }

    }
}