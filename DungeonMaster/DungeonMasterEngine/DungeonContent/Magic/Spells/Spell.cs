using DungeonMasterEngine.DungeonContent.Items;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
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