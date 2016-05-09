using System;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;

namespace DungeonMasterEngine.DungeonContent.GroupSupport
{
    public class GroupLayoutSearcher : BreadthFirstSearch<ISpace, object>
    {
        private LayoutManager layoutManager;

        public LayoutManager LayoutManager
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
            if (LayoutManager == null)
                throw new InvalidOperationException("Group server has to be set.");

            base.OnSearchStart();
        }

        protected override void AddSucessors(int layer, ISpace currentTile)
        {
            var spaces = currentTile.Neighbours
                .Select(t => t.Item1)
                .Where(layoutManager.IsFree);

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