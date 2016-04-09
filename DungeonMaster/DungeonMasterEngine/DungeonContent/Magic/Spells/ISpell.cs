using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public interface ISpell : IMovable<Tile>
    {
        void Run();
    }

    public abstract class Spell : LivingItem, ISpell
    {
        public abstract float TranslationVeloctiy { get; protected set; }

        public bool Finished { get; private set; } = false;


        protected Spell(Vector3 position) : base(position) { }

        public abstract void Run();

        public sealed override void Update(GameTime gameTime)
        {
            if (!Finished)
                OnSpellUpdate(gameTime);
        }

        protected abstract void OnSpellUpdate(GameTime gameTime);

        protected void FinishSpell()
        {
            Finished = true;
            Location = null;
        }
    }
}
