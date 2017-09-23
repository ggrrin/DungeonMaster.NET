using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Entity.Renderers
{
    class CreatureRenderer : MovableRenderer<Creature>
    {
        public CreatureRenderer(Creature movable, Texture2D face) : base(movable, face)
        {
            cube.Scale = new Vector3(0.3f, 0.7f, 0.3f);
            
        }
    }
}