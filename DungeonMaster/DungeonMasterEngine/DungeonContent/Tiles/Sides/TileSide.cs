using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.Tiles.Renderers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Tiles.Sides
{
    public class TileSide : ITileSide 
    {
        public MapDirection Face { get; }

        public TileSide(MapDirection face)
        {
            Face = face;
        }

        public IRenderer Renderer { get; set; }

        public virtual void AcceptMessage(Message message) { }

        
    }
}