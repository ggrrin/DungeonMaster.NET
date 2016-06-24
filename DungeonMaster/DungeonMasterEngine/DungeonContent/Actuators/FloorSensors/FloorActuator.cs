using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMasterEngine.DungeonContent.Actuators.WallSensors;
using DungeonMasterEngine.DungeonContent.Entity;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators.FloorSensors
{
    public class FloorActuator : ActuatorX
    {
        public override IEnumerable<SensorX> SensorsEnumeration => Sensors;
        public List<FloorSensor> Sensors { get; }
        //void F276_qzzz_SENSOR_ProcessThingAdditionOrRemoval(FloorTile currentTile, object thing, bool partyOnSquare, bool thingEntering)
        public void Trigger(object obj, IEnumerable<object> items, bool entering)
        {
            //THING L0766_T_Thing;
            //int L0767_i_ThingType;
            //SENSOR* L0769_ps_Sensor;
            //int L0770_ui_SensorTriggeredCell;
            //int L0771_ui_ThingType;
            //int L0774_i_ObjectType;
            //int L0777_ui_Square;

            //if (thing != C0xFFFF_THING_PARTY)
            //{
            //    L0767_i_ThingType = M12_TYPE(thing);
            //    L0774_i_ObjectType = F032_aaaY_OBJECT_GetType(thing);
            //}
            //else
            //{
            //    L0767_i_ThingType = CM1_THING_TYPE_PARTY;
            //    L0774_i_ObjectType = CM1_ICON_NONE;
            //}

            //if ((!thingEntering) && (L0767_i_ThingType != CM1_THING_TYPE_PARTY))
            //{
            //    F164_dzzz_DUNGEON_UnlinkThingFromList(thing, 0, P588_ui_MapX, P589_ui_MapY);
            //}

            bool alreadyContainsGrabable = items.Any(x => x is GrabableItem);
            bool alreadyContainsThingOfSameType = items.Any(x => x is GrabableItem && x.GetType().IsInstanceOfType(obj));
            bool alreadyContainsThingOfDifferentType = items.Any(x => x is GrabableItem && !x.GetType().IsInstanceOfType(obj));
            bool alreadyContainsCreature = items.Any(x => x is Creature);
            bool alreadyContainsParty = items.Any(x => x is ILeader);

            //L0766_T_Thing = F161_szzz_DUNGEON_GetSquareFirstThing(P588_ui_MapX, P589_ui_MapY);
            //while (L0766_T_Thing != C0xFFFE_THING_ENDOFLIST)
            //{
            //    if ((L0771_ui_ThingType = M12_TYPE(L0766_T_Thing)) == C04_THING_TYPE_GROUP)
            //    {
            //        L0773_B_SquareContainsGroup = TRUE;
            //    }
            //    else
            //    {
            //        if ((L0771_ui_ThingType == C02_THING_TYPE_TEXT) && (L0767_i_ThingType == CM1_THING_TYPE_PARTY) && thingEntering && !partyOnSquare)
            //        {
            //            F168_ajzz_DUNGEON_DecodeText(G353_ac_StringBuildBuffer, L0766_T_Thing, C1_TEXT_TYPE_MESSAGE);
            //            F047_xzzz_TEXT_MESSAGEAREA_PrintMessage(C15_COLOR_WHITE, G353_ac_StringBuildBuffer);
            //        }
            //        else
            //        {
            //            if ((L0771_ui_ThingType > C04_THING_TYPE_GROUP) && (L0771_ui_ThingType < C14_THING_TYPE_PROJECTILE))
            //            {
            //                L0772_B_SquareContainsObject = TRUE;
            //                L0775_B_SquareContainsThingOfSameType |= (F032_aaaY_OBJECT_GetType(L0766_T_Thing) == L0774_i_ObjectType);
            //                L0776_B_SquareContainsThingOfDifferentType |= (F032_aaaY_OBJECT_GetType(L0766_T_Thing) != L0774_i_ObjectType);
            //            }
            //        }
            //    }
            //    L0766_T_Thing = F159_rzzz_DUNGEON_GetNextThing(L0766_T_Thing);
            //}

            //if (thingEntering && (L0767_i_ThingType != CM1_THING_TYPE_PARTY))
            //{
            //    F163_amzz_DUNGEON_LinkThingToList(thing, 0, P588_ui_MapX, P589_ui_MapY);
            //}

            //for (L0766_T_Thing = F161_szzz_DUNGEON_GetSquareFirstThing(P588_ui_MapX, P589_ui_MapY); L0766_T_Thing != C0xFFFE_THING_ENDOFLIST; L0766_T_Thing = F159_rzzz_DUNGEON_GetNextThing(L0766_T_Thing))

            foreach (var item in Sensors)
            {
                item.TryInteract(this, obj, entering, alreadyContainsParty, alreadyContainsCreature, alreadyContainsGrabable, alreadyContainsThingOfSameType, alreadyContainsThingOfDifferentType);
            }
            F271_xxxx_SENSOR_ProcessRotationEffect(Sensors);
        }

        public FloorActuator(IEnumerable<FloorSensor> sensors)
        {
            if (sensors == null || sensors.Any(x => x == null))
                throw new ArgumentNullException();

            Sensors = new List<FloorSensor>(sensors);
        }

    }
}