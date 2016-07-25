using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Actuators.Renderers
{
    public class FountainRenderer : DecorationRenderer<Fountain>
    {
        public FountainRenderer(Texture2D decorationTexture, Fountain item) : base(decorationTexture, item) { }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            if (base.Interact(leader, ref currentTransformation, param))
            {
                Item.Trigger(leader);
                return true;
            }
            return false; ;
        }
    }
}