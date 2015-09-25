using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Graphics.ResourcesProvides
{
    class CubeResources : BufferResourceProvider
    {
        private static CubeResources instance;

        private VertexPositionNormalTexture[] vertices;
        private int[] indeices;

        public static new BufferResourceProvider Instance
        {
            get
            {
                if (instance == null)
                    instance = new CubeResources();

                return instance;
            }
        }

        private CubeResources() { if (IsInitialized) Initialize(); }

        protected override void Initialize()
        {
            base.Initialize();
            //Left/Right
            //Front/Back
            //Floor/Ceeling

            var lbf = Vector3.Zero;
            var rbf = Vector3.UnitX;
            var rff = new Vector3(1, 0, 1);
            var lff = Vector3.UnitZ;

            var lbc = lbf + Vector3.Up;
            var rbc = rbf + Vector3.Up;
            var rfc = rff + Vector3.Up;
            var lfc = lff + Vector3.Up;

            vertices = new VertexPositionNormalTexture[]
            {
                //back
                new VertexPositionNormalTexture(lbf, Vector3.UnitZ, new Vector2(0,1)),
                new VertexPositionNormalTexture(rbf, Vector3.UnitZ, new Vector2(1,1)),
                new VertexPositionNormalTexture(rbc, Vector3.UnitZ, new Vector2(1,0)),
                new VertexPositionNormalTexture(lbc, Vector3.UnitZ, new Vector2(0,0)),

                //right
                new VertexPositionNormalTexture(rbf, -Vector3.UnitX, new Vector2(0,1)),
                new VertexPositionNormalTexture(rbc, -Vector3.UnitX, new Vector2(0,0)),
                new VertexPositionNormalTexture(rfc, -Vector3.UnitX, new Vector2(1,0)),
                new VertexPositionNormalTexture(rff, -Vector3.UnitX, new Vector2(1,1)),
                
                //front
                new VertexPositionNormalTexture(rff, -Vector3.UnitZ, new Vector2(0,1)),
                new VertexPositionNormalTexture(lff, -Vector3.UnitZ, new Vector2(1,1)),
                new VertexPositionNormalTexture(lfc, -Vector3.UnitZ, new Vector2(1,0)),
                new VertexPositionNormalTexture(rfc, -Vector3.UnitZ, new Vector2(0,0)),

                //left
                new VertexPositionNormalTexture(lbf, Vector3.UnitX, new Vector2(1,1)),
                new VertexPositionNormalTexture(lbc, Vector3.UnitX, new Vector2(1,0)),
                new VertexPositionNormalTexture(lfc, Vector3.UnitX, new Vector2(0,0)),
                new VertexPositionNormalTexture(lff, Vector3.UnitX, new Vector2(0,1)),
                
                //floor
                new VertexPositionNormalTexture(lbf, Vector3.Up, new Vector2(0,0)),
                new VertexPositionNormalTexture(rbf, Vector3.Up, new Vector2(1,0)),
                new VertexPositionNormalTexture(rff, Vector3.Up, new Vector2(1,1)),
                new VertexPositionNormalTexture(lff, Vector3.Up, new Vector2(0,1)),

                //ceeling
                new VertexPositionNormalTexture(lbc, Vector3.Down, new Vector2(1,0)),
                new VertexPositionNormalTexture(rbc, Vector3.Down, new Vector2(0,0)),
                new VertexPositionNormalTexture(rfc, Vector3.Down, new Vector2(0,1)),
                new VertexPositionNormalTexture(lfc, Vector3.Down, new Vector2(1,1)),
            };

            indeices = new int[]
            {               
                //inner face visible
                0,2,3, 0,1,2, //back
                0,2,1, 0,3,2, //right
                0,1,2, 0,2,3, //front
                0,1,2, 0,2,3, //left
                0,2,1, 0,3,2, //floor
                0,1,2, 0,2,3,  //ceeling

                //outter face visible
                0,3,2, 0,2,1, //back
                0,1,2, 0,2,3, //right
                0,2,1, 0,3,2, //front
                0,2,1, 0,3,2, //left
                0,1,2, 0,2,3, //floor
                0,2,1, 0,3,2 //ceeling
            };
     

            IndexBuffer = new IndexBuffer(Device, typeof(int), indeices.Length, BufferUsage.None);
            IndexBuffer.SetData(indeices);

            VertexBuffer = new VertexBuffer(Device, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.None);
            VertexBuffer.SetData(vertices);
        }
    }
}