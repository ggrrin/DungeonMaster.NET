using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Graphics.ResourcesProvides;

namespace DungeonMasterEngine.Graphics
{
    public class GraphicsCollection : IGraphicProvider
    {
        private readonly List<IEnumerable<IWorldObject>> listOfdrawableList = new List<IEnumerable<IWorldObject>>();

        public IList<IGraphicProvider> SubProviders { get; }

        public IList<IWorldObject> SubDrawable { get; }  = new List<IWorldObject>();

        public GraphicsCollection()
        {
            SubProviders = new List<IGraphicProvider>();
        }

        public GraphicsCollection(params IGraphicProvider[] providers)
        {
            var list = new List<IGraphicProvider>(providers);
            SubProviders = list;
        }

        public void AddListOfDrawables(IEnumerable<IWorldObject> drawable)
        {
            listOfdrawableList.Add(drawable);
        }


        public BufferResourceProvider Resources 
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public void Draw(BasicEffect status)
        {
            //foreach (var i in listOfdrawableList)
            //    foreach (var g in i)
            //        g?.GraphicsProvider?.Draw(status);

            //foreach (var p in SubProviders)            
            //    p?.Draw(status);

            //foreach (var d in SubDrawable)
            //    d.GraphicsProvider?.Draw(status);
        }
    }
}
