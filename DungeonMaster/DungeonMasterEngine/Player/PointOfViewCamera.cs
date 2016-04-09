using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.Player
{
    public class PointOfViewCamera : FreeLookCamera, IPlayer, IMovable<Tile>
    {
        private readonly Animator<PointOfViewCamera, Tile> animator = new Animator<PointOfViewCamera, Tile>();
        private IPOVInputProvider inputProvider = new DefaultPOVInput();

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

        protected virtual void OnLocationChanged(Tile oldLocation, Tile newLocation)
        {
            LocationChanged?.Invoke(this, new LocationChangedEventArgs(oldLocation, newLocation));
        }

        public PointOfViewCamera(Game game) : base(game)
        {
            LocationChanged += (d, s) => $"{Location.Position} {Location.Neighbours}".Dump();
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

                if (translation.ToVector2().LengthSquared() == 1) //translation is not in oblique direction or zero
                {
                    var newLocation = location.Neighbours.GetTile(translation);

                    if (newLocation != null && newLocation.IsAccessible)
                    {
                        animator.MoveTo(this, newLocation);
                        return animator.GetTranslation(time);
                    }
                }

                return Vector3.Zero;
            }
        }

        private Point GetTranslation()
        {
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
                    return Point.Zero;
            }
            return GetShift(moveDirection);
        }

        public Point GetShift(Vector3 direction)
        {
            direction = Vector3.Normalize(new Vector3(direction.X, 0, direction.Z));//let direction be always horizontal of unit length
            direction += Position;//destination position
            return direction.ToGrid() - GridPosition;
        }

        private class DefaultPOVInput : IPOVInputProvider
        {
            public WalkDirection CurrentDirection
            {
                get
                {
                    var keyboardState = Keyboard.GetState();

                    if (keyboardState.IsKeyDown(Keys.A))
                        return WalkDirection.Left;

                    if (keyboardState.IsKeyDown(Keys.D))
                        return WalkDirection.Right;

                    if (keyboardState.IsKeyDown(Keys.S))
                        return WalkDirection.Backward;

                    if (keyboardState.IsKeyDown(Keys.W))
                        return WalkDirection.Forward;

                    return WalkDirection.None;
                }
            }
        }

        private class LocationChangedEventArgs : EventArgs
        {
            public Tile NewLocation { get; }
            public Tile OldLocation { get; }

            public LocationChangedEventArgs(Tile oldLocation, Tile newLocation)
            {
                OldLocation = oldLocation;
                NewLocation = newLocation;
            }
        }
    }
}