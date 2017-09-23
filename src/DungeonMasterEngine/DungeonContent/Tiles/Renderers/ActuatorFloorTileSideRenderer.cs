using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class ActuatorFloorTileSideRenderer : FloorTileSideRenderer<ActuatorFloorTileSide>
    {

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var res = base.Render(ref currentTransformation, effect, parameter);
            TileSide.Actuator.Renderer.Render(ref res, effect, parameter);
            return res;
        }

        public ActuatorFloorTileSideRenderer(ActuatorFloorTileSide tileSide, Texture2D wallTexture, Texture2D decorationTexture) : base(tileSide, wallTexture, decorationTexture) {}
    }
}