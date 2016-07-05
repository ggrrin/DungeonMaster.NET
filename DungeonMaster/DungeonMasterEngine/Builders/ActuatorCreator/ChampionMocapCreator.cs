using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity.BodyInventory.Base;
using DungeonMasterEngine.DungeonContent.Entity.Relations;
using DungeonMasterEngine.DungeonContent.Entity.Renderers;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.Graphics.ResourcesProvides;
using DungeonMasterEngine.Helpers;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.Builders.ActuatorCreator
{
    public class ChampionMocapCreator : ChampionCreator
    {
        public override ChampionBonesFactory BonesFactory { get; } = new ChampionBonesFactory("bones", 3, new IActionFactory[0], new IStorageType[0], new TextureRenderer(Matrix.Identity, null));

        public ChampionMocapCreator() : base(null)
        {
        }

        public Champion GetChampion(string descriptorData)
        {
            this.descriptor = descriptorData.Split('|');
            var res = new Champion(this, new RelationToken(0), new RelationToken(1).ToEnumerable())
            {
                Name = descriptor[0],
                Title = descriptor[1] + descriptor[2],
                IsMale = descriptor[3] == "M",
            };

            res.Renderer = new MovableRenderer<Champion>(res, ResourceProvider.Instance.DrawRenderTarget(res.Name, Color.Green, Color.White));
            return res;
        }
    }
}
