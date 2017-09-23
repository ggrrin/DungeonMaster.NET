using System.Collections.Generic;
using DungeonMasterEngine.Builders;
using DungeonMasterEngine.DungeonContent.Actions.Factories;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Skills.Base;
using DungeonMasterEngine.DungeonContent.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Spells;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;

namespace DungeonMasterEngine.Interfaces
{
    public interface IFactories
    {
        ushort MaxLight { get; } 
        IReadOnlyList<ushort> LightPowerToLightAmount { get; }
        IReadOnlyList<ushort> PaletteIndexToLightAmount { get; }
        IReadOnlyList<ISpellFactory<ISpell>> SpellFactories { get; }
        IReadOnlyList<ISpellSymbol> SpellSymbols { get; }
        IReadOnlyList<IPowerSymbol> PowerSymbol { get; }

        IReadOnlyList<ISkillFactory> Skills { get; }
        IReadOnlyList<HumanActionFactoryBase> FightActions { get; }
        IReadOnlyList<IReadOnlyList<IActionFactory>> ActionCombos { get; }
        
        IReadOnlyList<WeaponItemFactory> WeaponFactories { get; }
        IReadOnlyList<ClothItemFactory> ClothFactories { get; }
        IReadOnlyList<ContainerItemFactory> ContainerFactories { get; }
        IReadOnlyList<ScrollItemFactory> ScrollFactories { get; }
        IReadOnlyList<PotionFactory> PotionFactories { get; }
        IReadOnlyList<MiscItemFactory> MiscFactories { get; }
        IReadOnlyList<CreatureFactory> CreatureFactories { get; }
        IRenderersSource RenderersSource { get; }
    }
}