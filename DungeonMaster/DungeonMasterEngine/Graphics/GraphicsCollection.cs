using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using Microsoft.Xna.Framework.Graphics;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Graphics.ResourcesProvides;

namespace DungeonMasterEngine.Graphics
{
    public class GraphicsCollection : IGraphicProvider
    {
        private List<IEnumerable<WorldObject>> listOfdrawableList = new List<IEnumerable<WorldObject>>();

        public IList<IGraphicProvider> SubProviders { get; }

        public IList<WorldObject> SubDrawable { get; }  = new List<WorldObject>();

        public GraphicsCollection()
        {
            SubProviders = new List<IGraphicProvider>();
        }

        public GraphicsCollection(params IGraphicProvider[] providers)
        {
            var list = new List<IGraphicProvider>(providers);
            SubProviders = list;
        }

        public void AddListOfDrawables<T>(IEnumerable<T> drawable) where T : WorldObject // lalal I'm just using covariance :)))))
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
            foreach (var i in listOfdrawableList)
                foreach (var g in i)
                    g?.GraphicsProvider?.Draw(status);

            foreach (var p in SubProviders)            
                p?.Draw(status);

            foreach (var d in SubDrawable)
                d.GraphicsProvider?.Draw(status);
        }
    }
}
