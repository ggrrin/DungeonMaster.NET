using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.Graphics
{
    public abstract class Graphic : IGraphicProvider
    {
        private Matrix translationMatrix = Matrix.Identity;

        private Vector3 position = Vector3.Zero;
        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                if (position != value)
                {
                    transformationMatrixCache = null;
                    position = value;
                    translationMatrix = Matrix.CreateTranslation(position);
                }
            }
        }

        private Matrix scaleMatrix = Matrix.Identity;

        private Vector3 scale = new Vector3(1);
        public Vector3 Scale
        {
            get
            {
                return scale;
            }

            set
            {
                if (scale != value)
                {
                    transformationMatrixCache = null;
                    scale = value;
                    scaleMatrix = Matrix.CreateScale(scale);
                }
            }
        }

        private Matrix rotationMatrix = Matrix.Identity;

        private Vector3 rotation = new Vector3(1);

        /// <summary>
        /// Rotating around center of scaled moved object ( unit cubec assumed as default before any transformation)
        /// //TODO strangae 
        /// </summary>
        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                if (rotation != value)
                {
                    transformationMatrixCache = null;
                    rotation = value;
                    rotationMatrix = Matrix.CreateTranslation(-0.5f * scale) *
                        Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.Z, rotation.X) *
                        Matrix.CreateTranslation(0.5f * scale);
                }
            }
        }

        
        private Matrix mirrorMatrix = Matrix.Identity;

        public bool MirrorX
        {
            get
            {
                return mirrorMatrix.M11 == -1;
            }

            set
            {
                if (MirrorX != value)
                {
                    transformationMatrixCache = null;
                    mirrorMatrix.M11 *= -1;
                }
            }
        }

        public bool MirrorY
        {
            get
            {
                return mirrorMatrix.M22 == -1;
            }

            set
            {
                if (MirrorY != value)
                {
                    transformationMatrixCache = null;
                    mirrorMatrix.M22 *= -1;
                }
            }
        }

        public bool MirrorZ
        {
            get
            {
                return mirrorMatrix.M33 == -1;
            }

            set
            {
                if (MirrorZ != value)
                {
                    transformationMatrixCache = null;
                    mirrorMatrix.M33 *= -1;
                }
            }
        }

        private Matrix? transformationMatrixCache = Matrix.Identity;

        protected Matrix preTransformation = Matrix.Identity;

        protected Matrix transformationMatrix
        {
            get
            {
                if (transformationMatrixCache == null)
                {
                    return (Matrix)(transformationMatrixCache = preTransformation *
                        Matrix.CreateTranslation(-0.5f * scale) * mirrorMatrix * Matrix.CreateTranslation(0.5f * scale) 
                        * scaleMatrix * rotationMatrix * translationMatrix);
                }
                else
                {
                    return (Matrix)transformationMatrixCache;
                }
            }
        }    

        public abstract void Draw(BasicEffect status);
    }

}
