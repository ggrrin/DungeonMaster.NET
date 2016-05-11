using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using DungeonMasterEngine.Player;

namespace DungeonMasterEngine.Helpers
{
    public class Animator<Movable, Stopable> where Stopable : IStopable where Movable : IMovable<Stopable>
    {
        private Stopable newLocation, oldLocation;
        private Movable movableObject;
        private TaskCompletionSource<bool> animationPromise;
        private bool setLocation;

        public bool IsAnimating { get; private set; }
        private double time; //ms
        private Vector3 translation;
        private double timeDuration;

        public async Task MoveToAsync(Movable movableObject, Stopable newLocation, bool setLocation)
        {
            animationPromise = new TaskCompletionSource<bool>();
            MoveTo(movableObject, newLocation, setLocation);
            await animationPromise.Task;
        }

        public void MoveTo(Movable movableObject, Stopable newLocation, bool setLocation)
        {
            if (IsAnimating)
                FinsihAnimation();

            this.newLocation = newLocation;
            this.movableObject = movableObject;
            this.oldLocation = movableObject.Location;
            this.setLocation = setLocation;

            IsAnimating = true;
            time = 0;
            translation = this.newLocation.StayPoint - oldLocation.StayPoint;
            timeDuration =1000 * translation.Length() / movableObject.TranslationVelocity;
        }


        public void Update(GameTime gameTime)
        {
            if (IsAnimating)
            {
                time += gameTime.ElapsedGameTime.TotalMilliseconds;
                double timeFactor = time / timeDuration;

                if (timeFactor <= 1 && timeFactor > 0)
                {
                    movableObject.Position = oldLocation.StayPoint + translation * (float)timeFactor;
                }
                else
                {
                    FinsihAnimation();
                }
            }
        }

        private void FinsihAnimation()
        {
            if (setLocation)
                movableObject.Location = newLocation;

            movableObject.Position = newLocation.StayPoint;
            IsAnimating = false;
            animationPromise?.SetResult(true); 
        }
    }
}
