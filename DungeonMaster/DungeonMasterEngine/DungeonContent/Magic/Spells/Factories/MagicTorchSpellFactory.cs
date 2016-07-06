using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells.Factories
{
    public class MagicTorchSpellFactory : SpellFactory<MagicTorchSpell>
    {
        public IReadOnlyList<ushort> LightPowerToLightAmount { get; }


        public MagicTorchSpellFactory(SpellFactoryInitializer initializer, IReadOnlyList<ushort> lightPowerToLightAmount) : base(initializer)
        {
            this.LightPowerToLightAmount = lightPowerToLightAmount;
        }

        protected override MagicTorchSpell ApplySpellEffect(ILiveEntity entity, IPowerSymbol powerSymbol, int skillLevel)
        {
            return new MagicTorchSpell(powerSymbol, this);
        }

        
    }
}