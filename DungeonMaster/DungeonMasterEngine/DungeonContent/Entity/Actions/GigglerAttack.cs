namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public class GigglerAttack : CreatureAttack
    {
        public GigglerAttack(Creature attackProvider) : base(attackProvider) {}

        protected override bool F207_xxxx_GROUP_IsCreatureAttacking(ILiveEntity enemy)
        {
            F193_xxxx_GROUP_StealFromChampion(enemy);
            return false;
        }

        void F193_xxxx_GROUP_StealFromChampion(/*group P382_ps_Group,*/ ILiveEntity P383_i_ChampionIndex)
        {
            //int L0391_i_Percentage;
            //int L0392_i_StealFromSlotIndex;
            //int L0393_ui_Counter;
            //THING L0394_T_Thing;
            //Champion L0395_ps_Champion;
            //bool L0396_B_ObjectStolen;
            //char G394_auc_StealFromSlotIndices[8]
            //; /* Initialized with 0 bytes by C loader */


            //L0396_B_ObjectStolen = false;
            //L0391_i_Percentage = 100 - L0395_ps_Champion.GetProperty(PropertyFactory<DextrityProperty>.Instance).Value; //  F311_wzzz_CHAMPION_GetDexterity(L0395_ps_Champion = &G407_s_Party.Champions[P383_i_ChampionIndex]);
            //L0393_ui_Counter = random.Next(8);
            //while ((L0391_i_Percentage > 0) && 0 == L0395_ps_Champion.GetProperty(PropertyFactory<LuckProperty>.Instance).Value)
            ////!F308_vzzz_CHAMPION_IsLucky(L0395_ps_Champion, L0391_i_Percentage))
            //{
            //    if ((L0392_i_StealFromSlotIndex = G394_auc_StealFromSlotIndices[L0393_ui_Counter]) == C13_SLOT_BACKPACK_LINE1_1)
            //    {
            //        L0392_i_StealFromSlotIndex += random.Next(17); /* Select a random slot in the backpack */
            //    }
            //    if (((L0394_T_Thing = L0395_ps_Champion->Slots[L0392_i_StealFromSlotIndex]) != C0xFFFF_THING_NONE))
            //    {
            //        L0396_B_ObjectStolen = true;
            //        L0394_T_Thing = F300_aozz_CHAMPION_GetObjectRemovedFromSlot(P383_i_ChampionIndex, L0392_i_StealFromSlotIndex);
            //        if (P382_ps_Group->Slot == C0xFFFE_THING_ENDOFLIST)
            //        {
            //            P382_ps_Group->Slot = L0394_T_Thing; /* BUG0_12 An object is cloned and appears at two different locations in the dungeon and/or inventory. The game may crash when interacting with this object. If a Giggler with no possessions steals an object that was previously in a chest and was not the last object in the chest then the objects that followed it are cloned. In the chest, the object is part of a linked list of objects that is not reset when the object is removed from the chest and placed in the inventory (but not in the dungeon), nor when it is stolen and added as the first Giggler possession. If the Giggler already has a possession before stealing the object then this does not create a cloned object.
            //                    The following statement is missing: L0394_T_Thing->Next = C0xFFFE_THING_ENDOFLIST;
            //                    This creates cloned things if L0394_T_Thing->Next is not C0xFFFE_THING_ENDOFLIST which is the case when the object comes from a chest in which it was not the last object */
            //        }
            //        else
            //        {
            //            F163_amzz_DUNGEON_LinkThingToList(L0394_T_Thing, P382_ps_Group->Slot, CM1_MAPX_NOT_ON_A_SQUARE, 0);
            //        }
            //        F292_arzz_CHAMPION_DrawState(P383_i_ChampionIndex);
            //    }
            //    ++L0393_ui_Counter;
            //    L0393_ui_Counter &= 0x0007;
            //    L0391_i_Percentage -= 20;
            //}

            //if (0 == random.Next(8) || (L0396_B_ObjectStolen && random.Next(2) > 0))
            //{
            //    G375_ps_ActiveGroups[P382_ps_Group->ActiveGroupIndex].DelayFleeingFromTarget = random.Next(64) + 20;
            //    P382_ps_Group->Behavior = C5_BEHAVIOR_FLEE;
            //}
        }

    }
}