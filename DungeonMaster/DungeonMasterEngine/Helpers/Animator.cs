using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;

namespace DungeonMasterEngine.Helpers
{
    public class Animator<Movable, Stopable> where Stopable : IStopable where Movable : IMovable<Stopable>
    {
        public bool IsAnimating => NewLocation != null;
        public Stopable NewLocation { get; private set; }
        private Movable movableObject;
        private Vector3 animationTranslation;
        private TaskCompletionSource<bool> animationPromise;
        private bool setTargetLocation;

        public async Task MoveToAsync(Movable movableObject, Stopable newLocation)
        {
            MoveTo(movableObject, newLocation);

            animationPromise = new TaskCompletionSource<bool>();
            await animationPromise.Task;
        }

        public void MoveTo(Movable movableObject, Stopable newLocation, bool setTargetLocation = true)
        {
            if (IsAnimating)
                throw new InvalidOperationException("Animation in progress.");
            this.movableObject = movableObject;
            this.setTargetLocation = setTargetLocation;

            NewLocation = newLocation;

            animationTranslation = NewLocation.StayPoint - movableObject.Position;
        }

        public Vector3 GetTranslation(GameTime time)
        {
            if (!IsAnimating)
                throw new InvalidOperationException("Animation NOT in progress.");

            var translation = movableObject.TranslationVelocity * (float)time.ElapsedGameTime.TotalSeconds * Vector3.Normalize(animationTranslation);

            if (translation.LengthSquared() >= animationTranslation.LengthSquared())
            {
                return FinishAnimation();
            }
            else
            {//animate
                animationTranslation -= translation;
                return translation;
            }
        }

        private Vector3 FinishAnimation()
        {
            if (setTargetLocation)
                movableObject.Location = NewLocation;
            NewLocation = default(Stopable);//to notify animation stopped
            animationPromise?.SetResult(true);
            return animationTranslation;
        }

        public void QuickFinish()
        {
            if (IsAnimating)
            {
                FinishAnimation();
            }
        }

    }
}
