using DungeonMasterEngine.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items
{
    public class DoorItem : Item
    {
        public CubeGraphic Graphic => (CubeGraphic)Graphics;
        public bool HasButton { get; }

        public Vector3 Size => Graphic.Scale;

        public DoorItem(Vector3 position, bool hasButton) 
        {
            HasButton = hasButton;
            Graphics = new CubeGraphic { DrawFaces = CubeFaces.All, Outter = true, Scale = new Vector3(0.66f, 0.65f, 0.15f) };
        }
    }
}