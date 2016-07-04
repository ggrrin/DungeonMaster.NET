using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Tiles.Sides
{
    public class TileSide : ITileSide 
    {
        public MapDirection Face { get; }
        public bool RandomDecoration { get; }

        public TileSide(MapDirection face, bool randomDecoration)
        {
            Face = face;
            RandomDecoration = randomDecoration;
        }

        public IRenderer Renderer { get; set; }

        public virtual void AcceptMessage(Message message) { }

        
    }
}