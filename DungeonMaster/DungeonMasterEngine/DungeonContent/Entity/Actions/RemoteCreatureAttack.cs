namespace DungeonMasterEngine.DungeonContent.Entity.Actions
{
    public class RemoteCreatureAttack : CreatureAttack
    {
        public RemoteCreatureAttack(Creature attackProvider) : base(attackProvider) { }

        public override void ApplyAttack(MapDirection direction)
        {

            //int A0437_T_Thing;
            //switch (A0437_ui_CreatureType)
            //{
            //    case C14_CREATURE_VEXIRK:
            //    case C23_CREATURE_LORD_CHAOS:
            //        if (M05_RANDOM(2))
            //        {
            //            A0437_T_Thing = C0xFF80_THING_EXPLOSION_FIREBALL;
            //        }
            //        else
            //        {
            //            switch (M04_RANDOM(4))
            //            {
            //                case 0:
            //                    A0437_T_Thing = C0xFF83_THING_EXPLOSION_HARM_NON_MATERIAL;
            //                    break;
            //                case 1:
            //                    A0437_T_Thing = C0xFF82_THING_EXPLOSION_LIGHTNING_BOLT;
            //                    break;
            //                case 2:
            //                    A0437_T_Thing = C0xFF87_THING_EXPLOSION_POISON_CLOUD;
            //                    break;
            //                case 3:
            //                    A0437_T_Thing = C0xFF84_THING_EXPLOSION_OPEN_DOOR;
            //                    break;
            //            }
            //        }
            //        break;
            //    case C01_CREATURE_SWAMP_SLIME_SLIME_DEVIL:
            //        A0437_T_Thing = C0xFF81_THING_EXPLOSION_SLIME;
            //        break;
            //    case C03_CREATURE_WIZARD_EYE_FLYING_EYE:
            //        if (rand.Next(8) > 0)
            //        {
            //            A0437_T_Thing = C0xFF82_THING_EXPLOSION_LIGHTNING_BOLT;
            //        }
            //        else
            //        {
            //            A0437_T_Thing = C0xFF84_THING_EXPLOSION_OPEN_DOOR;
            //        }
            //        break;
            //    case C19_CREATURE_MATERIALIZER_ZYTAZ:
            //        if (rand.Next(2) > 0)
            //        {
            //            A0437_T_Thing = C0xFF87_THING_EXPLOSION_POISON_CLOUD;
            //            break;
            //        }
            //    case C22_CREATURE_DEMON:
            //    case C24_CREATURE_RED_DRAGON:
            //        A0437_T_Thing = C0xFF80_THING_EXPLOSION_FIREBALL;
            //        break;
            //} /* BUG0_13 The game may crash when 'Lord Order' or 'Grey Lord' cast spells. This cannot happen with the original dungeons as they do not contain any groups of these types. 'Lord Order' and 'Grey Lord' creatures can cast spells (attack range > 1) but no projectile type is defined for them in the code. If these creatures are present in a dungeon they will cast projectiles containing undefined things because the variable is not initialized */
            //int A0440_i_KineticEnergy = (L0441_ps_CreatureInfo->Attack >> 2) + 1;
            //A0440_i_KineticEnergy += rand.Next(A0440_i_KineticEnergy);
            //A0440_i_KineticEnergy += rand.Next(A0440_i_KineticEnergy);
            //F212_mzzz_PROJECTILE_Create(A0437_T_Thing, P422_i_MapX, P423_i_MapY, A0439_i_TargetCell, G382_i_CurrentGroupPrimaryDirectionToParty, F026_a003_MAIN_GetBoundedValue(20, A0440_i_KineticEnergy, 255), L0441_ps_CreatureInfo->Dexterity, 8);

        }



    }
}