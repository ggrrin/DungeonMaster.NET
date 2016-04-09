using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public class Door : Item
    {
        public CubeGraphic Graphic => (CubeGraphic)Graphics;
        public bool HasButton { get; }

        public Vector3 Size => Graphic.Scale;

        public Door(Vector3 position, bool hasButton) : base(position)
        {
            HasButton = hasButton;
            Graphics = new CubeGraphic { DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.66f, 0.65f, 0.15f) };
        }
    }
}