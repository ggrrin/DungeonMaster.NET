using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public interface IRenderer
    {
        Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter);
        bool Interact(ILeader leader, ref Matrix currentTransformation, object param);

        void Initialize();

        void Highlight(int miliseconds);
    }

    public abstract class Renderer : IRenderer
    {
        public abstract Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter);

        public abstract bool Interact(ILeader leader, ref Matrix currentTransformation, object param);

        public virtual void Initialize() { } 

        public bool Highlighted { get; set; }

        public virtual async void Highlight(int miliseconds)
        {
            Highlighted = true;

            await Task.Delay(miliseconds);

            Highlighted = false;
        }
    }
}