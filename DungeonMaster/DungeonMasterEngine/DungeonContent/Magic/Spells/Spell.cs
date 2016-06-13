using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public abstract class Spell :  ISpell
    {
        public Vector3 Position { get; set; }
        public abstract float TranslationVelocity { get; protected set; }

        public bool Finished { get; private set; } = false;


        protected Spell(Vector3 position) { } 

        public abstract void Run();

        public void Update(GameTime gameTime)
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

        public ITile Location { get; set; }
        public MapDirection MapDirection { get; set; }
    }
}