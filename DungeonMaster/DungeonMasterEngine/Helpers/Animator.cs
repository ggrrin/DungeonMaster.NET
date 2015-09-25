using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterEngine.Helpers
{
    public class Animator<Movable, Stopable> where Stopable : IStopable where Movable : IMovable<Stopable>
    {
        public bool IsAnimating { get { return NewLocation != null; } }

        public Stopable NewLocation { get; private set; }

        private Movable movableObject;

        private Vector3 animationTranslation;

        public void MoveTo(Movable movableObject, Stopable newLocation)
        {
            if (IsAnimating)
                throw new InvalidOperationException("Animation in progress.");
            this.movableObject = movableObject;

            NewLocation = newLocation;

            animationTranslation = NewLocation.StayPoint - movableObject.Position;
        }

        public Vector3 GetTranslation(GameTime time)
        {
            if (!IsAnimating)
                throw new InvalidOperationException("Animation NOT in progress.");

            var translation = movableObject.TranslationVeloctiy * (float)time.ElapsedGameTime.TotalSeconds * Vector3.Normalize(animationTranslation);

            if (translation.LengthSquared() >= animationTranslation.LengthSquared())
            {//finish animation
                movableObject.Location = NewLocation;
                NewLocation = default(Stopable);
                return Vector3.Zero;
            }
            else
            {//animate
                animationTranslation -= translation;
                return translation;
            }
        }
    }
}
