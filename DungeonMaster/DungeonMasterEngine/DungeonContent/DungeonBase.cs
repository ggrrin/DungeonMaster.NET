using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent
{
    public class DungeonBase<TLeader> : DungeonBase where TLeader : ILeader
    {
        public override ILeader LeaderBase => Leader;

        private TLeader leader;
        public virtual TLeader Leader
        {
            get { return leader; }
            protected set
            {
                if (Leader != null)
                {
                    Leader.LocationChanged -= Leader_LocationChanged;
                }
                leader = value;
                Leader.LocationChanged += Leader_LocationChanged;
            }
        }


        public DungeonBase(IDungonBuilder builder, GraphicsDevice graphicsDevice) : base(builder, graphicsDevice) { }
    }
}