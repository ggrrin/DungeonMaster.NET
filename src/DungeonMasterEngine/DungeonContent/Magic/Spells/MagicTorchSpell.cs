using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public class MagicTorchSpell : ISpell
    {
        private int duration;
        private int lightPower;
        public MagicTorchSpellFactory Factory { get; }

        public MagicTorchSpell(IPowerSymbol powerSymbol, MagicTorchSpellFactory factory)
        {
            Factory = factory;
            int spellPower = (powerSymbol.LevelOrdinal + 1) << 2;
            duration = 2000 + ((spellPower - 3) << 7);
            duration *= 1000/6;
            lightPower = (spellPower >> 2) + 1;
        }

        public async void Run(ILiveEntity caster, MapDirection direction)
        {
            var light = caster.GetProperty(PropertyFactory<MagicalLightProperty>.Instance);
            light.Value += Factory.LightPowerToLightAmount[lightPower];

            lightPower *= -1;

            while (true)
            {
                await Task.Delay(duration);

                if (lightPower == 0)
                    return;

                bool negative = lightPower < 0;
                if (negative)
                    lightPower *= -1;

                int weakerLightPower = lightPower - 1;
                int A0674_i_LightAmount = Factory.LightPowerToLightAmount[lightPower] - Factory.LightPowerToLightAmount[weakerLightPower];
                if (negative)
                {
                    A0674_i_LightAmount = -A0674_i_LightAmount;
                    weakerLightPower *= -1;
                }

                light.Value += A0674_i_LightAmount;
                if (weakerLightPower != 0)
                {
                    lightPower = weakerLightPower;
                    duration = 4000 / 6;
                }
                else
                    break;
            }
        }
    }
}