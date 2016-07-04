using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Magic.Spells;
using DungeonMasterEngine.DungeonContent.Magic.Spells.Factories;
using DungeonMasterEngine.DungeonContent.Magic.Symbols;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Magic
{
    public class SpellCastingManager : ISpellCastingManager
    {
        public ILiveEntity Entity { get; }
        protected List<ISpellSymbol> currentSequence = new List<ISpellSymbol>();

        public IEnumerable<ISpellSymbol> CurrentCastSequence => currentSequence;

        public IPowerSymbol CurrentPowerSymbol { get; protected set; }
        private IProperty mana => Entity.GetProperty(PropertyFactory<ManaProperty>.Instance);


        public SpellCastingManager(ILiveEntity entity)
        {
            Entity = entity;
        }


        public bool TryBeginCastSpell(IPowerSymbol powerSymbol)
        {
            currentSequence = new List<ISpellSymbol>();
            //var requiredMana = PowerSymbolFactories.Contains(powerSymbol);

            if (mana.Value < powerSymbol.ManaCostMultipler)
                return false;

            CurrentPowerSymbol = powerSymbol;
            mana.Value -= powerSymbol.ManaCostMultipler;
            return true;
        }

        public void ClearCastingSequence()
        {
            CurrentPowerSymbol = null;
            currentSequence = new List<ISpellSymbol>();
        }


        public bool TryCastSymbol(ISpellSymbol symbol)
        {
            if (CurrentPowerSymbol == null || CurrentPowerSymbol.LevelOrdinal > symbol.MaxSupportedLevel)
                return false;
            var requiredMana = (symbol.ManaCostsPerLevels[CurrentPowerSymbol.LevelOrdinal-1]) * CurrentPowerSymbol.ManaCostMultipler;
            requiredMana >>= 3;
            if (mana.Value < requiredMana)
                return false;

            mana.Value -= requiredMana;
            currentSequence.Add(symbol);
            return true;
        }

        public void RemoveSymbol()
        {
            if (currentSequence.Any())
                currentSequence.RemoveAt(currentSequence.Count);
        }

        public bool TryCastSpell(IEnumerable<ISpellFactory<ISpell>> spellFactories)
        {
            if (CurrentPowerSymbol == null || !currentSequence.Any())
                return false;

            var factory = spellFactories.FirstOrDefault(f => f.CastingSequence.SequenceEqual(currentSequence));
            factory?.CastSpell(CurrentPowerSymbol, Entity).Run(Entity, Entity.MapDirection);
            currentSequence = new List<ISpellSymbol>();
            CurrentPowerSymbol = null;
            return factory != null;
        }
    }

    //void F399_xxxx_MENUS_AddChampionSymbol(int P768_i_SymbolIndex)
    //{
    //    int L1224_i_SymbolIndex;

    //    CHAMPION* L1225_ps_Champion = &G407_s_Party.Champions[G514_i_MagicCasterChampionIndex];
    //    int L1222_ui_SymbolStep = L1225_ps_Champion->SymbolStep;
    //    int L1223_ui_ManaCost = G485_aauc_Graphic560_SymbolBaseManaCost[L1222_ui_SymbolStep][P768_i_SymbolIndex];
    //    if (L1222_ui_SymbolStep)
    //    {
    //        L1223_ui_ManaCost = (L1223_ui_ManaCost * G486_auc_Graphic560_SymbolManaCostMultiplier[L1224_i_SymbolIndex = L1225_ps_Champion->Symbols[0] - 96]) >> 3;
    //    }

    //    if (L1223_ui_ManaCost <= L1225_ps_Champion->CurrentMana)
    //    {
    //        L1225_ps_Champion->CurrentMana -= L1223_ui_ManaCost;
    //        M08_SET(L1225_ps_Champion->Attributes, MASK0x0100_STATISTICS);
    //        L1225_ps_Champion->Symbols[L1222_ui_SymbolStep] = 96 + (L1222_ui_SymbolStep * 6) + P768_i_SymbolIndex;
    //        L1225_ps_Champion->Symbols[L1222_ui_SymbolStep + 1] = '\0';
    //        L1225_ps_Champion->SymbolStep = L1222_ui_SymbolStep = M17_NEXT(L1222_ui_SymbolStep);
    //        F077_aA39_MOUSE_HidePointer_COPYPROTECTIONE();
    //        F397_xxxx_MENUS_DrawAvailableSymbols(L1222_ui_SymbolStep);
    //        F398_xxxx_MENUS_DrawChampionSymbols(L1225_ps_Champion);
    //        F292_arzz_CHAMPION_DrawState(G514_i_MagicCasterChampionIndex);
    //        F078_xzzz_MOUSE_ShowPointer();
    //    }
    //}

}

