using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DungeonMasterEngine.Graphics.ResourcesProvides
{
    public abstract class BufferResourceProvider : ResourceProvider
    {        
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        public VertexBuffer VertexBuffer
        {
            get { InitCheck(); return vertexBuffer; }
            protected set { vertexBuffer = value; }
        }

        public IndexBuffer IndexBuffer
        {
            get { InitCheck(); return indexBuffer; }
            protected set { indexBuffer = value; }
        }
    }
}