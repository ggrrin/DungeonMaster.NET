using System;
using Microsoft.Xna.Framework;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.Player
{
    public class PointOfViewCamera : FreeLookCamera, IMovable<ISpaceRouteElement>
    {
        private readonly Animator<PointOfViewCamera, ISpaceRouteElement> animator = new Animator<PointOfViewCamera, ISpaceRouteElement>();
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
            set
            {
                SetMapDirection(value, setForwardDirection: true);
            }
        }

        private void SetMapDirection(MapDirection value, bool setForwardDirection)
        {
            var oldDirection = mapDirection;
            if (mapDirection != value)
            {
                mapDirection = value;

                if (setForwardDirection)
                    ForwardDirection = new Vector3(mapDirection.RelativeShift.X, 0, mapDirection.RelativeShift.Y);

                OnMapDirectionChanged(oldDirection, mapDirection);
            }
        }

        public Point GridPosition => location.Tile.GridPosition;

        private ISpaceRouteElement location;

        public event EventHandler LocationChanged;

        public IGroupLayout Layout => FullTileLayout.Instance;

        public ISpaceRouteElement Location
        {
            get
            {
                return location;
            }
            set
            {
                var oldLocation = location;
                location = value;
                Position = location.Tile.StayPoint;

                if (location.Tile != oldLocation?.Tile)
                    OnLocationChanged(oldLocation?.Tile, location.Tile);
            }
        }

        public PointOfViewCamera()
        {
        }

        protected virtual void OnLocationChanged(ITile oldLocation, ITile newLocation)
        {
            LocationChanged?.Invoke(this, new LocationChangedEventArgs(oldLocation, newLocation));
            $"{Location.Tile.Position}".Dump();
        }

        protected virtual void OnLocationChanging(ITile oldLocation, ITile newLocation)
        { }

        protected virtual void OnMapDirectionChanged(MapDirection oldDirection, MapDirection newDirection)
        { }

        protected virtual bool CanMoveToTile(ITile tile)
        {
            return tile != null && tile.IsAccessible;
        }

        protected override Vector3 GetTranslation(GameTime time)
        {
            animator.Update(time);

            if (animator.IsAnimating)
                return Vector3.Zero;

            Point? translation = GetTranslation();
            if (translation != null)
            {
                var newLocation = location.Tile.Neighbors.GetTile(new MapDirection(translation.Value));
                if (CanMoveToTile(newLocation))
                {
                    OnLocationChanging(Location.Tile, newLocation);
                    animator.MoveTo(this, Layout.GetSpaceElement(OnethSpace.Instance, newLocation), setLocation: true);
                }
            }

            return Vector3.Zero;
        }

        public void MoveTo(ITile newLocation, bool setNewLocation)
        {
            OnLocationChanging(Location.Tile, newLocation);
            animator.MoveTo(this, Layout.GetSpaceElement(OnethSpace.Instance, newLocation), setNewLocation);
        }

        private Point? GetTranslation()
        {
            Point? shift = GetShift(ForwardDirection);
            if (shift != null)
                SetMapDirection(new MapDirection(shift.Value), setForwardDirection: false);

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