using DungeonMasterEngine.Graphics.ResourcesProvides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class DecorationResource : BufferResourceProvider
    {
        public new static DecorationResource Instance { get; } = new DecorationResource();

        public BoundingBox BoundingBox { get; }

        private DecorationResource()
        {
            const float r= 0.5f;
            var lbf = new Vector3(-r, -r, 0);
            var rbf = new Vector3(r, -r, 0);
            var lbc = lbf + Vector3.Up;// * r;
            var rbc = rbf + Vector3.Up ;//* r;

            BoundingBox = new BoundingBox(lbf - Vector3.UnitZ * 0.05f, rbc);

            var vertices = new VertexPositionNormalTexture[]
            {
                new VertexPositionNormalTexture(lbf, Vector3.UnitZ, new Vector2(0, 1)),
                new VertexPositionNormalTexture(rbf, Vector3.UnitZ, new Vector2(1, 1)),
                new VertexPositionNormalTexture(rbc, Vector3.UnitZ, new Vector2(1, 0)),
                new VertexPositionNormalTexture(lbc, Vector3.UnitZ, new Vector2(0, 0)),
            };

            var indeices = new int[]
            {
                0,
                2,
                3,
                0,
                1,
                2,
            };

            IndexBuffer = new IndexBuffer(Device, typeof(int), indeices.Length, BufferUsage.None);
            IndexBuffer.SetData(indeices);

            VertexBuffer = new VertexBuffer(Device, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.None);
            VertexBuffer.SetData(vertices);
        }
    }
}