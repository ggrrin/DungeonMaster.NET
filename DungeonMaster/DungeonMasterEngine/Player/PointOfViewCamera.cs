using System;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.Player
{
    public class PointOfViewCamera : FreeLookCamera, IPlayer, IMovable<Tile>
    {
        private readonly Animator<PointOfViewCamera, Tile> animator = new Animator<PointOfViewCamera, Tile>();
        private IPOVInputProvider inputProvider = new DefaultPOVInput();
        private MapDirection mapDirection = MapDirection.South;

        public IPOVInputProvider InputProvider
        {
            get { return inputProvider; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                inputProvider = value;
            }
        }

        public MapDirection MapDirection
        {
            get
            {
                return mapDirection;
            }
            private set
            {
                var oldDirection = mapDirection;
                if (mapDirection != value)
                {
                    mapDirection = value;
                    OnMapDirectionChanged(oldDirection, mapDirection);
                }
            }
        }

        public Point GridPosition => location.GridPosition;

        private Tile location;

        public event EventHandler LocationChanged;

        public Tile Location
        {
            get
            {
                return location;
            }
            set
            {
                var oldLocation = location;
                location = value;
                Position = location.StayPoint;

                if (location != oldLocation)
                    OnLocationChanged(oldLocation, location);
            }
        }

        public PointOfViewCamera(Game game) : base(game)
        {
            LocationChanged += (d, s) => $"{Location.Position} {Location.Neighbours}".Dump();
        }

        protected virtual void OnLocationChanged(Tile oldLocation, Tile newLocation)
        {
            LocationChanged?.Invoke(this, new LocationChangedEventArgs(oldLocation, newLocation));
        }

        protected virtual void OnLocationChanging(Tile oldLocation, Tile newLocation)
        { }

        protected virtual void OnMapDirectionChanged(MapDirection oldDirection, MapDirection newDirection)
        { }

        protected virtual bool CanMoveToTile(Tile tile)
        {
            return tile != null && tile.IsAccessible;
        }

        protected override Vector3 GetTranslation(GameTime time)
        {
            if (animator.IsAnimating)
            {
                return animator.GetTranslation(time);
            }
            else
            {
                var translation = GetTranslation();
                if (translation != null)
                {
                    var newLocation = location.Neighbours.GetTile(new MapDirection(translation.Value));
                    if (CanMoveToTile(newLocation))
                    {
                        OnLocationChanging(Location, newLocation);
                        animator.MoveTo(this, newLocation);
                        return animator.GetTranslation(time);
                    }
                }
            }

            return Vector3.Zero;
        }

        private Point? GetTranslation()
        {
            Point? shift = GetShift(ForwardDirection);
            if (shift != null)
                MapDirection = new MapDirection(shift.Value);

            Vector3 moveDirection = Vector3.Zero;

            switch (inputProvider.CurrentDirection)
            {
                case WalkDirection.Forward:
                    moveDirection = ForwardDirection;
                    break;

                case WalkDirection.Backward:
                    moveDirection = BackwardDirection;
                    break;

                case WalkDirection.Left:
                    moveDirection = LeftDirection;
                    break;

                case WalkDirection.Right:
                    moveDirection = RighDirection;
                    break;

                default:
                    return null;
            }
            return GetShift(moveDirection);
        }

        private Point? GetShift(Vector3 direction)
        {
            if (direction == Vector3.Zero)
                return null;

            direction = Vector3.Normalize(new Vector3(direction.X, 0, direction.Z));//let direction be always horizontal of unit length
            direction += Position;//destination position
            var translation = direction.ToGrid() - GridPosition;

            if (translation.ToVector2().LengthSquared() != 1) //translation is in oblique direction or zero
                return null;
            else
                return translation;
        }
    }
}