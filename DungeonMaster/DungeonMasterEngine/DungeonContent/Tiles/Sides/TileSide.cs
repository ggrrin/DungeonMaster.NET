using DungeonMasterEngine.DungeonContent.Actuators.Wall;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class TileSide   : IRenderable
    {
        public MapDirection Face { get; }
        public bool RandomDecoration { get; }

        public TileSide(MapDirection face, bool randomDecoration)
        {
            Face = face;
            RandomDecoration = randomDecoration;
        }

        public Renderer Renderer { get; set; }

        public virtual void SendMessage(Message message) { }

        public Interactor Interactor { get; set; }
    }

}