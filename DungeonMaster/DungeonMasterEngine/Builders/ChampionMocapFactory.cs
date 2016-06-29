using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
using DungeonMasterEngine.DungeonContent.Entity.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Graphics;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders
{
    public class ChampionMocapFactory : ChampionFactory
    {
        public ChampionMocapFactory() : base(null) { }

        public Champion GetChampion(string descriptorData)
        {
            this.descriptor = descriptorData.Split('|');
            var res = new Champion(this, new RelationToken(0), new RelationToken(1).ToEnumerable())
            {
                Name = descriptor[0],
                Title = descriptor[1] + descriptor[2],
                IsMale = descriptor[3] == "M",
            };

            res.Renderer = new LiveEntityRenderer<Champion>(res, ResourceProvider.Instance.DrawRenderTarget(res.Name, Color.Green, Color.White));
            return res;
        }
    }
}
