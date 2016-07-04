using System;
using System.Linq;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base
{
    public class GroupLayoutSearcher : BreadthFirstSearch<ISpace, object>
    {
        private LayoutManager<ILiveEntity> layoutManager;

        public LayoutManager<ILiveEntity> LayoutManager
        {
            get { return layoutManager; }
            set
            {
                if (Searching)
                    throw new InvalidOperationException("Cannot change when searching");
                layoutManager = value;
            }
        }

        protected override void OnSearchStart()
        {
            //removed to support searching on all spaces
            //if (LayoutManager == null)
            //    throw new InvalidOperationException("Group server has to be set.");

            base.OnSearchStart();
        }

        protected override void AddSucessors(int layer, ISpace currentTile)
        {
            var spaces = currentTile.Neighbors
                .Select(t => t.Item1)
                .Where(s => LayoutManager?.IsFree(s) ?? true);//if layout manager not specified use all spaces

            foreach (var s in spaces)
                Enqueue(s, layer, currentTile);
        }

        protected override void OnSearchFinished()
        {
            base.OnSearchFinished();
            LayoutManager = null;
        }
    }
}