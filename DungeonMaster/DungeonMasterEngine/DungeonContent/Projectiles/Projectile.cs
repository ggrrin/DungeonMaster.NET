using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Projectiles
{
    public abstract class Projectile : IMovable<ISpaceRouteElement>, IRenderable
    {
        protected static readonly Random rand = new Random();

        private int kineticEnergy;
        private int attack;


        public int KineticEnergy
        {
            get { return kineticEnergy; }
            protected set { kineticEnergy = value <= 0 ? 0 : value; }
        }

        public int StepEnergy { get; }

        public int Attack
        {
            get { return attack; }
            protected set { attack = value <= 0 ? 0 : value; }
        }

        protected readonly Animator<Projectile, ISpaceRouteElement> animator = new Animator<Projectile, ISpaceRouteElement>();
        private ISpaceRouteElement location;
        public Vector3 Position { get; set; }
        public float TranslationVelocity => 7.4f;
        public MapDirection MapDirection { get; set; }
        public MapDirection Direction { get; protected set; }
        public virtual IGroupLayout Layout => Small4GroupLayout.Instance;
        public bool Active { get; protected set; } = true;

        public ISpaceRouteElement Location
        {
            get { return location; }
            set
            {
                if (location == value)
                    throw new InvalidOperationException("Cannot move from space to itself.");

                bool alreadyOnTile = location?.Tile == value.Tile;
                bool differentLevel = location?.Tile?.Level != value?.Tile?.Level;
                if (location != null && differentLevel)
                    Active = false;

                if (!alreadyOnTile)
                    location?.Tile.Drawables.Remove(this);

                location = value;
                Position = location.StayPoint;

                if (!alreadyOnTile)
                    location?.Tile.Drawables.Add(this);
            }
        }



        protected Projectile(int kineticEnergy, int stepEnergy, int attack)
        {
            KineticEnergy = kineticEnergy;
            Attack = attack;
            StepEnergy = stepEnergy;
        }

        public async void Run(ISpaceRouteElement casterSpace, MapDirection direction)
        {
            Direction = direction;
            Location = GetInitialLocation(casterSpace, direction);
            Location.Tile.Level.Updateables.Add(this);
            //cost of the move before party
            KineticEnergy -= StepEnergy;
            Attack -= StepEnergy;

            while (Active)
            {
                var newSpace = Location.Space.Neighbors.FirstOrDefault(neighbour => neighbour.Item2 == direction)?.Item1;
                if (newSpace != null)//move to space
                {
                    if (!await Move(newSpace, Location.Tile))
                        break;
                }
                else
                {//move to tile
                    var newTile = Location.Tile.Neighbors.GetTile(Direction);
                    if (newTile == null)
                        break;

                    var route = Layout.GetToNeighbour(Location, newTile, true);
                    if (route.Count() != 2)
                        throw new InvalidOperationException("Invalid path, target location should be neighbor.");

                    if (!await Move(route.ElementAt(1).Space, newTile))
                        break;

                    if (KineticEnergy <= 0)
                        break;
                }
            }
            Destory();
        }
        private void Destory()
        {
            Active = false;
            location.Tile.Level.Updateables.Remove(this);
            location.Tile.Drawables.Remove(this);
            animator.AbortFinishAsync();
            FinishImpact();
        }

        protected abstract void FinishImpact();

        protected virtual bool TryApplyAfterMoving() => false;

        protected virtual bool TryApplyBeforeMoving() => false;

        protected virtual ISpaceRouteElement GetInitialLocation(ISpaceRouteElement casterSpace, MapDirection direction)
        {
            var targetTile = casterSpace.Tile.Neighbors.GetTile(direction);
            if (targetTile != null)//TODO what is inaccessible ? 
            {
                //find space on next tile in direction direction, which is adjacent to party tile
                var angularSides = casterSpace.Space.Sides.Where(s => s != direction && s != direction.Opposite) //remove sides, which are in line with direction
                    .ToArray();

                var randomAngularSide = angularSides[rand.Next(angularSides.Length)];
                var spaceConstrainDirection = new[]
                {
                    randomAngularSide,
                    direction.Opposite
                };

                var fullStisfactionSpaces = Layout.AllSpaces.Where(s => spaceConstrainDirection.All(s.Sides.Contains)).ToArray();

                if (fullStisfactionSpaces.Length == 0)
                {
                    fullStisfactionSpaces = Layout.AllSpaces.Where(s => spaceConstrainDirection.Any(s.Sides.Contains)).ToArray();

                    if (fullStisfactionSpaces.Length == 0)
                        throw new InvalidOperationException("There should be always some spaces adjacent to sides.");
                }
                var winSpace = fullStisfactionSpaces[rand.Next(fullStisfactionSpaces.Length)];
                return Layout.GetSpaceElement(winSpace, targetTile);
            }
            else
            {
                //smash it right on caster space
                return GetClosestSpace(casterSpace, Layout.AllSpaces.Select(x => Layout.GetSpaceElement(x, Location.Tile)));
            }
        }

        private async Task<bool> Move(ISpace space, ITile location)
        {
            if (TryApplyBeforeMoving())
                return false;

            await animator.MoveToAsync(this, Layout.GetSpaceElement(space, location), true);

            KineticEnergy -= StepEnergy;
            Attack -= StepEnergy;

            if (TryApplyAfterMoving())
                return false;

            return true;
        }


        public void Update(GameTime time)
        {
            animator.Update(time);
        }

        protected virtual ISpaceRouteElement GetClosestSpace(ISpaceRouteElement sourceSpace, IEnumerable<ISpaceRouteElement> possibleSpaces)
        {
            return possibleSpaces
                .Where(sr => sr.Space.Area.Intersects(sourceSpace.Space.Area))//filter only intersecting spaces
                .MinObj(sr => Vector3.Distance(sr.StayPoint, sourceSpace.StayPoint));//if ambiguous select closer to stay point
        }

        public void MoveTo(ITile newLocation, bool setNewLocation)
        {
            throw new NotImplementedException();
        }

        public IRenderer Renderer { get; set; }
    }
}