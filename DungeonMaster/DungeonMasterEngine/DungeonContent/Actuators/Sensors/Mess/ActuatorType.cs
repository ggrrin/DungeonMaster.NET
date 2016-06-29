namespace DungeonMasterEngine.DungeonContent.Actuators.Sensors.Mess
{
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
}