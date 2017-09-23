using DungeonMasterEngine.DungeonContent.Tiles.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles.Renderers
{
    public interface IRenderer
    {
        Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter);

        Matrix GetCurrentTransformation(ref Matrix parentTransformation);

        bool Interact(ILeader leader, ref Matrix currentTransformation, object param);

        void Initialize();

        void Highlight(int miliseconds);

        bool Highlighted { get; set; }
    }
}