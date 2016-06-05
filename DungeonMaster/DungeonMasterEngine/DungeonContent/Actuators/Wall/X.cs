using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Entity.Skills;
using DungeonMasterEngine.DungeonContent.Entity.Skills.@base;
using DungeonMasterEngine.DungeonContent.GroupSupport;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems;
using DungeonMasterEngine.DungeonContent.Items.GrabableItems.Factories;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Player;
using static DungeonMasterEngine.DungeonContent.Actuators.Wall.SensorEffect;

namespace DungeonMasterEngine.DungeonContent.Actuators.Wall
{
    public enum EarthSides
    {
        CM1_CELL_ANY = -1,
        C00_CELL_NORTHWEST = 0,
        C01_CELL_NORTHEAST = 1,
        C02_CELL_SOUTHEAST = 2,
        C03_CELL_SOUTHWEST = 3
    }

    public enum SensorEffect
    {
        CM1_EFFECT_NONE = -1
        , C00_EFFECT_SET = 0
        , C01_EFFECT_CLEAR = 1
        , C02_EFFECT_TOGGLE = 2
        , C03_EFFECT_HOLD = 3
        , C10_EFFECT_ADD_EXPERIENCE = 10
    }

    enum ActuatorType
    {
        C000_SENSOR_DISABLED = 0 /* Never triggered, may be used for a floor or wall ornament */
        //, C001_SENSOR_FLOOR_THERON_PARTY_CREATURE_OBJECT                                =1 /* Triggered by party/thing F276_qzzz_SENSOR_ProcessThingAdditionOrRemoval */
        //, C002_SENSOR_FLOOR_THERON_PARTY_CREATURE                                       =2 /* Triggered by party/thing F276_qzzz_SENSOR_ProcessThingAdditionOrRemoval */
        //, C003_SENSOR_FLOOR_PARTY                                                       =3 /* Triggered by party/thing F276_qzzz_SENSOR_ProcessThingAdditionOrRemoval */
        //, C004_SENSOR_FLOOR_OBJECT                                                      =4 /* Triggered by party/thing F276_qzzz_SENSOR_ProcessThingAdditionOrRemoval */
        //, C005_SENSOR_FLOOR_PARTY_ON_STAIRS                                             =5 /* Triggered by party/thing F276_qzzz_SENSOR_ProcessThingAdditionOrRemoval */
        //, C006_SENSOR_FLOOR_GROUP_GENERATOR                                             =6 /* Triggered by event F245_xxxx_TIMELINE_ProcessEvent5_Square_Corridor */
        //, C007_SENSOR_FLOOR_CREATURE                                                    =7 /* Triggered by party/thing F276_qzzz_SENSOR_ProcessThingAdditionOrRemoval */
        //, C008_SENSOR_FLOOR_PARTY_POSSESSION                                            =8 /* Triggered by party/thing F276_qzzz_SENSOR_ProcessThingAdditionOrRemoval */
        //, C009_SENSOR_FLOOR_VERSION_CHECKER                                             =9 /* Triggered by party/thing F276_qzzz_SENSOR_ProcessThingAdditionOrRemoval */

        , C001_SENSOR_WALL_ORNAMENT_CLICK = 1 /* Triggered by player click F275_aszz_SENSOR_IsTriggeredByClickOnWall */
        , C002_SENSOR_WALL_ORNAMENT_CLICK_WITH_ANY_OBJECT = 2 /* Triggered by player click F275_aszz_SENSOR_IsTriggeredByClickOnWall */
        , C003_SENSOR_WALL_ORNAMENT_CLICK_WITH_SPECIFIC_OBJECT = 3 /* Triggered by player click F275_aszz_SENSOR_IsTriggeredByClickOnWall */
        , C004_SENSOR_WALL_ORNAMENT_CLICK_WITH_SPECIFIC_OBJECT_REMOVED = 4 /* Triggered by player click F275_aszz_SENSOR_IsTriggeredByClickOnWall */
        , C005_SENSOR_WALL_AND_OR_GATE = 5 /* Triggered by event F248_xxxx_TIMELINE_ProcessEvent6_Square_Wall */
        , C006_SENSOR_WALL_COUNTDOWN = 6 /* Triggered by event F248_xxxx_TIMELINE_ProcessEvent6_Square_Wall */
        , C007_SENSOR_WALL_SINGLE_PROJECTILE_LAUNCHER_NEW_OBJECT = 7 /* Triggered by event F248_xxxx_TIMELINE_ProcessEvent6_Square_Wall */
        , C008_SENSOR_WALL_SINGLE_PROJECTILE_LAUNCHER_EXPLOSION = 8 /* Triggered by event F248_xxxx_TIMELINE_ProcessEvent6_Square_Wall */
        , C009_SENSOR_WALL_DOUBLE_PROJECTILE_LAUNCHER_NEW_OBJECT = 9 /* Triggered by event F248_xxxx_TIMELINE_ProcessEvent6_Square_Wall */

        , C010_SENSOR_WALL_DOUBLE_PROJECTILE_LAUNCHER_EXPLOSION = 10 /* Triggered by event F248_xxxx_TIMELINE_ProcessEvent6_Square_Wall */
        , C011_SENSOR_WALL_ORNAMENT_CLICK_WITH_SPECIFIC_OBJECT_REMOVED_ROTATE_SENSORS = 11 /* Triggered by player click F275_aszz_SENSOR_IsTriggeredByClickOnWall */
        , C012_SENSOR_WALL_OBJECT_GENERATOR_ROTATE_SENSORS = 12 /* Triggered by player click F275_aszz_SENSOR_IsTriggeredByClickOnWall */
        , C013_SENSOR_WALL_SINGLE_OBJECT_STORAGE_ROTATE_SENSORS = 13 /* Triggered by player click F275_aszz_SENSOR_IsTriggeredByClickOnWall */
        , C014_SENSOR_WALL_SINGLE_PROJECTILE_LAUNCHER_SQUARE_OBJECT = 14 /* Triggered by event F248_xxxx_TIMELINE_ProcessEvent6_Square_Wall */
        , C015_SENSOR_WALL_DOUBLE_PROJECTILE_LAUNCHER_SQUARE_OBJECT = 15 /* Triggered by event F248_xxxx_TIMELINE_ProcessEvent6_Square_Wall */
        , C016_SENSOR_WALL_OBJECT_EXCHANGER = 16 /* Triggered by player click F275_aszz_SENSOR_IsTriggeredByClickOnWall */
        , C017_SENSOR_WALL_ORNAMENT_CLICK_WITH_SPECIFIC_OBJECT_REMOVED_REMOVE_SENSOR = 17 /* Triggered by player click F275_aszz_SENSOR_IsTriggeredByClickOnWall */
        , C018_SENSOR_WALL_END_GAME = 18 /* Triggered by event F248_xxxx_TIMELINE_ProcessEvent6_Square_Wall */
        , C127_SENSOR_WALL_CHAMPION_PORTRAIT = 127 /* Triggered by player click F275_aszz_SENSOR_IsTriggeredByClickOnWall */
    }

    struct APart1
    {
        public uint Unreferenced;//:2;
        public bool OnceOnly;//:1;
        public SensorEffect Effect;//:2; /* Not used for group generators */
        public bool RevertEffect;//:1; /* Not used for group generators */
        public bool Audible;//:1;
        public IGrabableItemFactoryBase Value;//:4; /* Ticks for all sensors except group generators (where Bit 10: 0 fixed number, 1 random number and Bits 9-7: count) and end game (where the value is a delay in seconds) */
        public bool LocalEffect;//:1; /* Not used for group generators */
        public uint OrnamentOrdinal;//:4;
    }

    struct BUninon
    {
        public BA A;
        public BB B;
    }

    struct BA
    { /* Regular with remote target */
        public uint Unreferenced;//:4;
        public EarthSides TargetCell;//:2; /* Ignored for squares other than walls */
        public int TargetMapX;//:5;
        public int TargetMapY;//:5;
    }

    struct BB
    { /* Regular with local target, Launcher, Group generators */
        public uint Unreferenced;//:4;
        public uint Multiple;//:12;
    }




    class SubXC001 : Sensor
    {
        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;
            if (Effect == C03_EFFECT_HOLD)
            {
                return false;
            }
            return true;
        }

    }

    class SubXc002 : Sensor
    {
        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = ((theron.Hand == null) != RevertEffect);
            return true;
        }
    }

    abstract class SubXc017c011 : Sensor
    {
        //process actutator
        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            if (!isLast)
                return false;

            L0753_B_DoNotTriggerSensor = ((Data == theron.Hand.Factory) == RevertEffect);

            if (!L0753_B_DoNotTriggerSensor)
                theron.Hand = null;

            return true;
        }
    }

    class SubXc017 : SubXc017c011
    {
        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            if (!base.Interact(theron, actuator, isLast, out L0753_B_DoNotTriggerSensor))
                return false;

            if (!L0753_B_DoNotTriggerSensor)
            {
                if (actuator.Sensors.Count == 1)
                    return true;

                actuator.Sensors.Remove(this);
            }
            return true;
        }
    }

    class SubXc011 : SubXc017c011
    {
        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            if (!base.Interact(theron, actuator, isLast, out L0753_B_DoNotTriggerSensor))
                return false; ;

            if (!L0753_B_DoNotTriggerSensor)
            {
                F270_xxxx_SENSOR_TriggerLocalEffect(theron, actuator, true); /* This will cause a rotation of the sensors at the specified cell on the specified square after all sensors have been processed */
            }
            return true;
        }
    }



    class SubXc004 : Sensor
    {
        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = ((Data == theron.Hand.Factory) == RevertEffect);
            if (!L0753_B_DoNotTriggerSensor)
                theron.Hand = null;
            return true; ;
        }
    }


    class SubXc003 : Sensor
    {
        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = ((Data == theron.Hand.Factory) == RevertEffect);
            return true; ;
        }
    }

    class SubXYc012 : Sensor
    {
        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            if (!isLast)
                return false;

            L0753_B_DoNotTriggerSensor = theron.Hand != null;
            if (!L0753_B_DoNotTriggerSensor)
            {
                F270_xxxx_SENSOR_TriggerLocalEffect(theron, actuator, true); /* This will cause a rotation of the sensors at the specified cell on the specified square after all sensors have been processed */
                theron.Hand = Data.Create();
            }
            return true;
        }
    }

    class SubSyc013 : Sensor
    {
        public IGrabableItem Storage { get; private set; }

        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            if (theron.Hand == null)
            {
                if ((theron.Hand = Storage) == null)
                    return false;

                Storage = null;
            }
            else
            {
                if ((theron.Hand.Factory != Data) || Storage != null)
                    return false;

                Storage = theron.Hand;
                theron.Hand = null;
            }

            F270_xxxx_SENSOR_TriggerLocalEffect(theron, actuator, true); /* This will cause a rotation of the sensors at the specified cell on the specified square after all sensors have been processed */
            if ((Effect == C03_EFFECT_HOLD) && theron.Hand != null)
            {
                L0753_B_DoNotTriggerSensor = true;
            }
            else
            {
                L0753_B_DoNotTriggerSensor = false;
            }
            return true;
        }
    }

    class SubXYc016 : Sensor
    {
        public IGrabableItem Storage { get; private set; }

        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            if (!isLast)
            { /* If the sensor is not the last one of its type on the cell */
                return false;
            }
            //F162_afzz_DUNGEON_GetSquareFirstObject(); //TODO is it possible to put item from one side of the wall and take it from antoher ? 
            if ((theron.Hand.Factory != Data) || (Storage == null))
                return false;

            var handItem = theron.Hand;
            theron.Hand = Storage;
            Storage = handItem;
            L0753_B_DoNotTriggerSensor = false;
            return true;
        }
    }

    class SubXYc127 : Sensor
    {
        public override bool TryTrigger(Theron theron, ActuatorX actuator, bool isLast)
        {
            if (theron.Leader == null)
                return false;

            return base.TryTrigger(theron, actuator, isLast);
        }

        protected override bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor)
        {
            L0753_B_DoNotTriggerSensor = false;//Doesnt matter
            //TODO add add
            throw new NotImplementedException();
            return false;
        }

    }


    public abstract class Sensor
    {
        public SensorEffect Effect { get; }
        //local target
        public bool ExperienceGain { get; }
        public bool Rotate { get; }

        //remote target
        public EarthSides Specifer { get; }
        public Tile TargetTile { get; }

        public int TimeDelay { get; }
        public bool LocalEffect { get; }
        public bool RevertEffect { get; }
        public bool OnceOnly { get; }
        public bool Audible { get; }

        public bool Disabled { get; private set; }

        public IGrabableItemFactoryBase Data { get; }

        protected abstract bool Interact(Theron theron, ActuatorX actuator, bool isLast, out bool L0753_B_DoNotTriggerSensor);

        public virtual bool TryTrigger(Theron theron, ActuatorX actuator, bool isLast)
        {
            bool notTriggered;

            if (Disabled)
                return false;

            if (!Interact(theron, actuator, isLast, out notTriggered))
                return false;

            var usedEffect = Effect;
            if (usedEffect == C03_EFFECT_HOLD)
            {
                usedEffect = notTriggered ? C01_EFFECT_CLEAR : C00_EFFECT_SET;
                notTriggered = false;
            }

            if (!notTriggered)
            {
                if (Audible)
                {
                    //TODO play audio 
                    //F064_aadz_SOUND_RequestPlay_COPYPROTECTIOND(C01_SOUND_SWITCH, G306_i_PartyMapX, G307_i_PartyMapY, C01_MODE_PLAY_IF_PRIORITIZED);
                }

                if (OnceOnly)
                    Disabled = true;

                F272_xxxx_SENSOR_TriggerEffect(theron, actuator, usedEffect);
            }

            return !notTriggered;
        }


        async void F272_xxxx_SENSOR_TriggerEffect(Theron theron, ActuatorX actuator, SensorEffect P576_i_Effect)
        {
            if (LocalEffect)
            {
                F270_xxxx_SENSOR_TriggerLocalEffect(theron, actuator);
            }
            else
            {
                //TODO send B,A,TargetCell only to wall tiles, floor tiles has set this flag NORTHWEST
                //TODO time delay await Task.Delay(A.Value);
                await Task.Delay(TimeDelay);
                //TODO send message
            }
        }

        protected void F270_xxxx_SENSOR_TriggerLocalEffect(Theron theron, ActuatorX actuator, bool? rotate = null)
        {
            if (ExperienceGain)
            {
                //TODO on floor tiles add skill to all party members
                F269_xxxx_SENSOR_AddSkillExperience(theron, SkillFactory<StealSkill>.Instance, 300, true /*P574_i_SensorLocalEffectCell != CM1_CELL_ANY*/);
            }
            else if (rotate ?? Rotate)
            {
                actuator.Rotate = Rotate;
            }
        }


        void F269_xxxx_SENSOR_AddSkillExperience(Theron theron, ISkillFactory skill, int exprience, bool P570_B_LeaderOnly)
        {
            if (P570_B_LeaderOnly)
            {
                theron.Leader.GetSkill(skill).AddExperience(exprience);
                return;
            }
            exprience /= theron.PartyGroup.Count;

            foreach (var champion in theron.PartyGroup)
            {
                //TODO champion not/alive
                champion.GetSkill(skill).AddExperience(exprience);
            }
        }

    }

    public abstract class ActuatorX
    {
        public List<Sensor> Sensors { get; }
        public bool Rotate { get; set; }
        public Tile Tile { get; }
        public TileSide Side { get; }


        bool F275_aszz_SENSOR_IsTriggeredByClickOnWall(Theron theron)
        {
            bool anyTriggered = false;

            foreach (var sensor in Sensors)
                anyTriggered = anyTriggered || sensor.TryTrigger(theron, this, sensor == Sensors.Last());

            F271_xxxx_SENSOR_ProcessRotationEffect();
            return anyTriggered;
        }


        void F271_xxxx_SENSOR_ProcessRotationEffect()
        {
            if (!Rotate)
                return;

            var first = Sensors.FirstOrDefault();
            if (first != null)
            {
                Sensors.RemoveAt(0);
                Sensors.Add(first);
            }

            Rotate = false;
        }
    }


}
