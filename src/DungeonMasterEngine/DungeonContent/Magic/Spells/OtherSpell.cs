using DungeonMasterEngine.DungeonContent.Entity;

namespace DungeonMasterEngine.DungeonContent.Magic.Spells
{
    public class OtherSpell : Spell
    {
        protected void ApplySpellEffect(Champion L1270_ps_Champion, Spell L1271_ps_Spell, int L1268_i_PowerSymbolOrdinal, int A1267_ui_SkillLevel)
        {
            //EVENT L1276_s_Event;
            //L1276_s_Event.A.A.Priority = 0;
            //int A1267_ui_SpellPower = (L1268_i_PowerSymbolOrdinal + 1) << 2;
            //int A1267_ui_LightPower;
            //int A1267_ui_Ticks;
            //int A1269_ui_Ticks;
            //switch (M68_SPELL_TYPE(L1271_ps_Spell))
            //{
            //    case C0_SPELL_TYPE_OTHER_LIGHT:
            //        A1269_ui_Ticks = 10000 + ((A1267_ui_SpellPower - 8) << 9);
            //        A1267_ui_LightPower = (A1267_ui_SpellPower >> 1);
            //        A1267_ui_LightPower--;
            //        G407_s_Party.MagicalLightAmount += G039_ai_Graphic562_LightPowerToLightAmount[A1267_ui_LightPower];
            //        F404_xxxx_MENUS_CreateEvent70_Light(-A1267_ui_LightPower, A1269_ui_Ticks);
            //        break;
            //    case C5_SPELL_TYPE_OTHER_MAGIC_TORCH:
            //        A1269_ui_Ticks = 2000 + ((A1267_ui_SpellPower - 3) << 7);
            //        A1267_ui_LightPower = (A1267_ui_SpellPower >> 2);
            //        A1267_ui_LightPower++;
            //        G407_s_Party.MagicalLightAmount += G039_ai_Graphic562_LightPowerToLightAmount[A1267_ui_LightPower];
            //        F404_xxxx_MENUS_CreateEvent70_Light(-A1267_ui_LightPower, A1269_ui_Ticks);
            //        break;
            //    case C1_SPELL_TYPE_OTHER_DARKNESS:
            //        A1267_ui_LightPower = (A1267_ui_SpellPower >> 2);
            //        G407_s_Party.MagicalLightAmount -= G039_ai_Graphic562_LightPowerToLightAmount[A1267_ui_LightPower];
            //        F404_xxxx_MENUS_CreateEvent70_Light(A1267_ui_LightPower, 98);
            //        break;
            //    case C2_SPELL_TYPE_OTHER_THIEVES_EYE:
            //        L1276_s_Event.A.A.Type = C73_EVENT_THIEVES_EYE;
            //        G407_s_Party.Event73Count_ThievesEye++;
            //        A1267_ui_SpellPower = (A1267_ui_SpellPower >> 1);
            //        goto T412_032;
            //    case C3_SPELL_TYPE_OTHER_INVISIBILITY:
            //        L1276_s_Event.A.A.Type = C71_EVENT_INVISIBILITY;
            //        G407_s_Party.Event71Count_Invisibility++;
            //        goto T412_033;
            //    case C4_SPELL_TYPE_OTHER_PARTY_SHIELD:
            //        L1276_s_Event.A.A.Type = C74_EVENT_PARTY_SHIELD;
            //        L1276_s_Event.B.Defense = A1267_ui_SpellPower;
            //        if (G407_s_Party.ShieldDefense > 50)
            //        {
            //            L1276_s_Event.B.Defense >>= 2;
            //        }
            //        G407_s_Party.ShieldDefense += L1276_s_Event.B.Defense;
            //        F260_pzzz_TIMELINE_RefreshAllChampionStatusBoxes();
            //        goto T412_032;
            //    case C6_SPELL_TYPE_OTHER_FOOTPRINTS:
            //        L1276_s_Event.A.A.Type = C79_EVENT_FOOTPRINTS;
            //        G407_s_Party.Event79Count_Footprints++;
            //        G407_s_Party.FirstScentIndex = G407_s_Party.ScentCount;
            //        if (L1268_i_PowerSymbolOrdinal < 3)
            //        {
            //            G407_s_Party.LastScentIndex = G407_s_Party.FirstScentIndex;
            //        }
            //        else
            //        {
            //            G407_s_Party.LastScentIndex = 0;
            //        }
            //        T412_032:
            //        A1267_ui_Ticks = A1267_ui_SpellPower * A1267_ui_SpellPower;
            //        T412_033:
            //        M33_SET_MAP_AND_TIME(L1276_s_Event.Map_Time, G309_i_PartyMapIndex, G313_ul_GameTime + A1267_ui_Ticks);
            //        F238_pzzz_TIMELINE_AddEvent_GetEventIndex_COPYPROTECTIONE(&L1276_s_Event);
            //        break;
            //    case C7_SPELL_TYPE_OTHER_ZOKATHRA:
            //        if ((L1272_T_Object = F166_szzz_DUNGEON_GetUnusedThing(C10_THING_TYPE_JUNK)) == C0xFFFF_THING_NONE)
            //        {
            //            break;
            //        }
            //        JUNK L1277_ps_Junk = (JUNK*)F156_afzz_DUNGEON_GetThingData(L1272_T_Object);
            //        L1277_ps_Junk->Type = C51_JUNK_ZOKATHRA;
            //        int A1267_ui_SlotIndex;
            //        if (L1270_ps_Champion->Slots[C00_SLOT_READY_HAND] == C0xFFFF_THING_NONE)
            //        {
            //            A1267_ui_SlotIndex = C00_SLOT_READY_HAND;
            //        }
            //        else
            //        {
            //            if (L1270_ps_Champion->Slots[C01_SLOT_ACTION_HAND] == C0xFFFF_THING_NONE)
            //            {
            //                A1267_ui_SlotIndex = C01_SLOT_ACTION_HAND;
            //            }
            //            else
            //            {
            //                A1267_ui_SlotIndex = -1;
            //            }
            //        }
            //        if ((A1267_ui_SlotIndex == C00_SLOT_READY_HAND) || (A1267_ui_SlotIndex == C01_SLOT_ACTION_HAND))
            //        {
            //            F301_apzz_CHAMPION_AddObjectInSlot(P795_i_ChampionIndex, L1272_T_Object, A1267_ui_SlotIndex);
            //            F292_arzz_CHAMPION_DrawState(P795_i_ChampionIndex);
            //        }
            //        else
            //        {
            //            F267_dzzz_MOVE_GetMoveResult_COPYPROTECTIONCE(L1272_T_Object, CM1_MAPX_NOT_ON_A_SQUARE, 0, G306_i_PartyMapX, G307_i_PartyMapY);
            //        }
            //        break;
            //    case C8_SPELL_TYPE_OTHER_FIRESHIELD:
            //        F403_xxxx_MENUS_IsPartySpellOrFireShieldSuccessful(L1270_ps_Champion, FALSE, (A1267_ui_SpellPower * A1267_ui_SpellPower) + 100, FALSE);
            //}
        }

        public override void Run(ILiveEntity caster, MapDirection direction)
        {
            throw new System.NotImplementedException();
        }
    }
}