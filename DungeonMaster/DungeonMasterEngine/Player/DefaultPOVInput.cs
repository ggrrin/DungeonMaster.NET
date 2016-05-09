using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace DungeonMasterEngine.Player
{
    public class DefaultPOVInput : IPOVInputProvider
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
}