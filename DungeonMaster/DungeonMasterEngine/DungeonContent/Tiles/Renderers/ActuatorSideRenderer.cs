using DungeonMasterEngine.DungeonContent.Tiles.Sides;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public class ActuatorSideRenderer : TileWallSideRenderer<ActuatorWallTileSide>
    {
        public ActuatorSideRenderer(ActuatorWallTileSide side, Texture2D wallTexture) : base(side, wallTexture) { }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            var finalTransFormation = base.Render(ref currentTransformation, effect, parameter);

            TileSide.Actuator.Renderer.Render(ref finalTransFormation, effect, parameter);
            return finalTransFormation;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            Matrix finalTransformation;
            Matrix.Multiply(ref transformation, ref currentTransformation, out finalTransformation);
            return TileSide.Actuator.Renderer.Interact(leader, ref finalTransformation, param);
        }
    }
}