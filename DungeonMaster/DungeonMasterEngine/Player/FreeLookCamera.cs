using DungeonMasterEngine.Graphics.ResourcesProvides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.Player
{
    public class FreeLookCamera : IViewStatus
    {
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
        protected Point sceenCenter => new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

        public float TranslationVelocity { get; set; } = 4.4f;

        public Matrix View { get; private set; }

        public Matrix Projection { get; private set; }

        public float FieldOfView { get; set; } = MathHelper.PiOver2;
        public GraphicsDevice GraphicsDevice { get; }

        public FreeLookCamera()
        {
            GraphicsDevice = ResourceProvider.Instance.Device;
            Mouse.SetPosition(sceenCenter.X, sceenCenter.Y);
            Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100);
        }


        public virtual void Update(GameTime time)
        {
            var move = GetTranslation(time);
            if (move != Vector3.Zero)
                Position += TranslationVelocity * (float)time.ElapsedGameTime.TotalSeconds * Vector3.Normalize(move);//same speed for oblique direction 

            var rotation = GetRotation(time);

            //TODO bounding up down look
            //if (sign(horizontal.X) != sign(Direction.X) || sign(horizontal.Z) != sign(Direction.Z))
            //    Direction = horizontal;

            ForwardDirection = Vector3.Transform(ForwardDirection, rotation);
            ForwardDirection.Normalize();
            var t = ForwardDirection;
            const float val = 0.93f;
            if (t.Y > val)
                t.Y = val;
            if (t.Y < -val)
                t.Y = -val;

            t.Normalize();
            ForwardDirection = t;

            View = Matrix.CreateLookAt(Position, Position + ForwardDirection, Up);
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
                verticalAngle = mouseMove.X / GraphicsDevice.Viewport.Width * FieldOfView;
                horizontalAngle = mouseMove.Y / GraphicsDevice.Viewport.Width * FieldOfView;
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

            //horizontalAngle = MathHelper.Clamp(horizontalAngle, -0.23f, 0.23f);

            Quaternion rotationUp = Quaternion.CreateFromAxisAngle(Up, verticalAngle);
            Quaternion rotationLeft = Quaternion.CreateFromAxisAngle(-Vector3.Normalize(Vector3.Cross(ForwardDirection, Up)), horizontalAngle);

            return Quaternion.Concatenate(rotationUp, rotationLeft);
        }
    }
}

