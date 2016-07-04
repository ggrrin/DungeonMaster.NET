using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public class OpenDoorProjectileFactory : ProjectilSpellFactory<OpenDoorSpell>
    {
        public OpenDoorProjectileFactory(SpellFactoryInitializer initializer) : base(initializer) { }

        protected override  OpenDoorSpell ApplySpellEffect(ILiveEntity l1270PsChampion, IPowerSymbol l1268IPowerSymbolOrdinal, int a1267UiSkillLevel)
        {
            a1267UiSkillLevel <<= 1;
            int kineticEnergy = MathHelper.Clamp(21, (l1268IPowerSymbolOrdinal.LevelOrdinal + 2) * (4 + (a1267UiSkillLevel << 1)), 255);
            var values = F327_kzzz_CHAMPION_IsProjectileSpellCast(l1270PsChampion, kineticEnergy, 0);
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