using System;
using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Helpers;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public abstract class NoesFireballProjectile
    {

        private static void potionException()//THING L0486_T_ProjectileAssociatedThing, ref GROUP* L0491_ps_Group, ref POTION* L0492_ps_Potion, ref THING L0498_T_ExplosionThing, ref int L0508_i_PotionPower, ref bool L0509_B_RemovePotion, int L0510_i_ProjectileAssociatedThingType)
        {
            //if (L0510_i_ProjectileAssociatedThingType == C08_THING_TYPE_POTION)
            //{
            //    L0491_ps_Group = (GROUP*)F156_afzz_DUNGEON_GetThingData(L0486_T_ProjectileAssociatedThing);
            //    switch (((POTION*)L0491_ps_Group)->Type)
            //    {
            //        case C03_POTION_VEN_POTION:
            //            L0498_T_ExplosionThing = C0xFF87_THING_EXPLOSION_POISON_CLOUD;
            //            goto T217_004;
            //        case C19_POTION_FUL_BOMB:
            //            L0498_T_ExplosionThing = C0xFF80_THING_EXPLOSION_FIREBALL;
            //            T217_004:
            //            L0509_B_RemovePotion = TRUE;
            //            L0508_i_PotionPower = ((POTION*)L0491_ps_Group)->Power;
            //            L0492_ps_Potion = (POTION*)L0491_ps_Group;
            //    }
            //}
        }


        private void itemAbsorbtion()
        {
            //if (!L0505_B_CreateExplosionOnImpact && (A0487_i_Outcome == C0_OUTCOME_KILLED_NO_CREATURES_IN_GROUP) && (L0510_i_ProjectileAssociatedThingType == C05_THING_TYPE_WEAPON) && M07_GET(L0493_ps_CreatureInfo->Attributes, MASK0x0400_KEEP_THROWN_SHARP_WEAPONS))
            //{
            //    L0495_ps_Weapon = (WEAPON*)F156_afzz_DUNGEON_GetThingData(L0486_T_ProjectileAssociatedThing);
            //    A0487_i_WeaponType = L0495_ps_Weapon->Type;
            //    if ((A0487_i_WeaponType == C08_WEAPON_DAGGER) || (A0487_i_WeaponType == C27_WEAPON_ARROW) || (A0487_i_WeaponType == C28_WEAPON_SLAYER) || (A0487_i_WeaponType == C31_WEAPON_POISON_DART) || (A0487_i_WeaponType == C32_WEAPON_THROWING_STAR))
            //    {
            //        L0497_pT_GroupSlot = &L0491_ps_Group->Slot;
            //    }
            //}
        }

        void exceptions()
        {
            ////black flame exception
            //if ((L0486_T_ProjectileAssociatedThing == C0xFF80_THING_EXPLOSION_FIREBALL) && (L0511_i_CreatureType == C11_CREATURE_BLACK_FLAME))
            //{
            //    L0496_pui_CreatureHealth = &L0491_ps_Group->Health[L0512_i_CreatureIndex];
            //    *L0496_pui_CreatureHealth = F024_aatz_MAIN_GetMinimumValue(1000, *L0496_pui_CreatureHealth + F216_xxxx_PROJECTILE_GetImpactAttack(L0490_ps_Projectile, L0486_T_ProjectileAssociatedThing));
            //    return; //goto T217_044;
            //}
        }

        void x()
        {
            ////TODO open door spell 
            //A0487_i_DoorState = M36_DOOR_STATE(L0503_uc_Square = G271_ppuc_CurrentMapData[AP454_i_ProjectileTargetMapX][AP455_i_ProjectileTargetMapY]);
            //L0494_ps_Door = (DOOR*)F157_rzzz_DUNGEON_GetSquareFirstThingData(AP454_i_ProjectileTargetMapX, AP455_i_ProjectileTargetMapY);
            //if ((A0487_i_DoorState != C5_DOOR_STATE_DESTROYED) && (L0486_T_ProjectileAssociatedThing == C0xFF84_THING_EXPLOSION_OPEN_DOOR))
            //{
            //    if (L0494_ps_Door->Button)
            //    {
            //        F268_fzzz_SENSOR_AddEvent(C10_EVENT_DOOR, AP454_i_ProjectileTargetMapX, AP455_i_ProjectileTargetMapY, 0, C02_EFFECT_TOGGLE, G313_ul_GameTime + 1);
            //    }
            //    break;
            //}

            ////TODO throwing item through doors
            //if ((A0487_i_DoorState == C5_DOOR_STATE_DESTROYED) || (A0487_i_DoorState <= C1_DOOR_STATE_CLOSED_ONE_FOURTH) || (M07_GET(G275_as_CurrentMapDoorInfo[L0494_ps_Door->Type].Attributes, MASK0x0002_PROJECTILES_CAN_PASS_THROUGH) && ((L0510_i_ProjectileAssociatedThingType == C15_THING_TYPE_EXPLOSION) ? (L0486_T_ProjectileAssociatedThing >= C0xFF83_THING_EXPLOSION_HARM_NON_MATERIAL) : ((L0490_ps_Projectile->Attack > M03_RANDOM(128)) && M07_GET(G237_as_Graphic559_ObjectInfo[F141_anzz_DUNGEON_GetObjectInfoIndex(L0486_T_ProjectileAssociatedThing)].AllowedSlots, MASK0x0100_POUCH_PASS_AND_THROUGH_DOORS) && ((L0510_i_ProjectileAssociatedThingType != C10_THING_TYPE_JUNK) || ((A0487_i_IconIndex = F033_aaaz_OBJECT_GetIconIndex(L0486_T_ProjectileAssociatedThing)) < 0) || (!((A0487_i_IconIndex >= C176_ICON_JUNK_IRON_KEY) && (A0487_i_IconIndex <= C191_ICON_JUNK_MASTER_KEY))))))))
            //{
            //    return FALSE;
            //}
        }

       
    }
}