using System;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public class OpenDoorSpell : Spell
    {
        private readonly Point startDirection;
        private readonly Animator<OpenDoorSpell, Tile> animator = new Animator<OpenDoorSpell, Tile>();

        public override float TranslationVeloctiy { get; protected set; } = 2 * 2.2f;

        public OpenDoorSpell(Tile location, Point startDirection) : base(location.Position)
        {
            this.startDirection = startDirection;
            Location = location;
        }


        public override void Run()
        {
            if (animator.IsAnimating)
                throw new InvalidOperationException();
            StartNextMove();
        }

        private void StartNextMove()
        {
            var moveTile = Location.Neighbours.GetTile(startDirection);
            if (moveTile != null)
                animator.MoveTo(this, moveTile);
            else
                FinishSpell();
        }

        protected override void OnSpellUpdate(GameTime gameTime)
        {
            if (animator.IsAnimating)
                animator.GetTranslation(gameTime);
            else
            {
                if (!TryFinishSpell())
                    StartNextMove();
                else
                    FinishSpell();
            }
        }

        private bool TryFinishSpell()
        {
            $"{GetType().Name} : {Location.Position}".Dump();
            var door = Location as Door;
            if (door != null && door.HasButton)
            {
                door.ActivateTileContent();
                return true;
            }
            else if(!Location.IsAccessible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}