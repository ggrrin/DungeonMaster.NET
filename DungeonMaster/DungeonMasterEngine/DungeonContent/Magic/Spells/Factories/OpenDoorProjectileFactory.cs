using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public class OpenDoorProjectileFactory : ProjectilSpellFactory<OpenDoorSpell>
    {
        public OpenDoorProjectileFactory(SpellFactoryInitializer initializer) : base(initializer) { }

        protected override  OpenDoorSpell ApplySpellEffect(ILiveEntity entity, IPowerSymbol powerSymbol, int skillLevel)
        {
            skillLevel <<= 1;
            int kineticEnergy = MathHelper.Clamp(21, (powerSymbol.LevelOrdinal + 2) * (4 + (skillLevel << 1)), 255);
            var values = F327_kzzz_CHAMPION_IsProjectileSpellCast(entity, kineticEnergy, 0);
            if (values != null)
            {
                var val = values.Value;
                return new OpenDoorSpell(val.KineticEnergy, val.StepEnergy);
            }
            else
            {
                return null;
            }
        }



    }
}