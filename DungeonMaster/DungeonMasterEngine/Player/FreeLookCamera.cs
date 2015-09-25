
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.Player
{
    public class FreeLookCamera : GameComponent, IViewStatus
    {
        //TODO IInputProvider

        //TODO normalize inside setter
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Up { get; set; } = Vector3.Up;

        //always normalized
        private Vector3 forwardDirection = Vector3.UnitZ;

        public Ray Ray => new Ray(Position, forwardDirection);

        public Vector3 ForwardDirection
        {
            get { return forwardDirection; }
            set { forwardDirection = Vector3.Normalize(value); }
        }

        public Vector3 BackwardDirection => -ForwardDirection; 

        public Vector3 LeftDirection => -Vector3.Cross(ForwardDirection, Up); 

        public Vector3 RighDirection => -LeftDirection; 
        protected Point sceenCenter => new Point(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2); 

        public float TranslationVeloctiy { get; set; } = 2 * 2.2f;


        public Matrix View { get; private set; }

        public Matrix Projection { get; private set; }

        public float FieldOfView { get; set; } = MathHelper.PiOver2;

        public FreeLookCamera(Game game) : base(game)
        {
            Mouse.SetPosition(sceenCenter.X, sceenCenter.Y);
            Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, Game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100);
        }

        public override void Update(GameTime time)
        {
            if (Game.IsActive)
            {
                var move = GetTranslation(time);
                if (move != Vector3.Zero)
                    Position += TranslationVeloctiy * (float)time.ElapsedGameTime.TotalSeconds * Vector3.Normalize(move);//same speed for oblique direction 

                var rotation = GetRotation(time);

                //TODO bounding up down look
                //if (sign(horizontal.X) != sign(Direction.X) || sign(horizontal.Z) != sign(Direction.Z))
                //    Direction = horizontal;

                ForwardDirection = Vector3.Transform(ForwardDirection, rotation);
                ForwardDirection.Normalize();

                View = Matrix.CreateLookAt(Position, Position + ForwardDirection, Up);

            }
            base.Update(time);
        }

        protected virtual Vector3 GetTranslation(GameTime time)
        {
            var move = Vector3.Zero;

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.A))
                move += LeftDirection;

            if (keyboardState.IsKeyDown(Keys.D))
                move += RighDirection;

            if (keyboardState.IsKeyDown(Keys.S))
                move += BackwardDirection;

            if (keyboardState.IsKeyDown(Keys.W))
                move += ForwardDirection;

            return move;
        }

        protected virtual Quaternion GetRotation(GameTime time)
        {
            var coor = Mouse.GetState().Position;
            
            Vector2 mouseMove = new Vector2(sceenCenter.X - coor.X, coor.Y - sceenCenter.Y);
            Mouse.SetPosition(sceenCenter.X, sceenCenter.Y);

            float verticalAngle = 0;
            float horizontalAngle = 0;
            if (mouseMove != Vector2.Zero)
            {
                verticalAngle = mouseMove.X / Game.GraphicsDevice.Viewport.Width * FieldOfView;
                horizontalAngle = mouseMove.Y / Game.GraphicsDevice.Viewport.Width * FieldOfView;
            }
            else
            {
                KeyboardState keys = Keyboard.GetState();
                const float move = 0.03f;
                if (keys.IsKeyDown(Keys.Left))
                    verticalAngle = move;
                if (keys.IsKeyDown(Keys.Right))
                    verticalAngle = -move;
                if (keys.IsKeyDown(Keys.Up))
                    horizontalAngle = -move;
                if (keys.IsKeyDown(Keys.Down))
                    horizontalAngle = move;
            }
            
            Quaternion rotationUp = Quaternion.CreateFromAxisAngle(Up, verticalAngle);
            Quaternion rotationLeft = Quaternion.CreateFromAxisAngle(-Vector3.Normalize(Vector3.Cross(ForwardDirection, Up)), horizontalAngle);



            return Quaternion.Concatenate(rotationUp, rotationLeft);
        }
    }
}

