using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Entity.Renderers
{
    class CreatureRenderer : LiveEntityRenderer<Creature>
    {
        public CreatureRenderer(Creature entity, Texture2D face) : base(entity, face)
        {
            cube.Scale = new Vector3(0.3f, 0.7f, 0.3f);
            
        }
    }
}