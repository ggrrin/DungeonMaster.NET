using DungeonMasterParser.Enums;
using DungeonMasterParser.Support;

namespace DungeonMasterParser.Items
{
    public class ActuatorItem : SuperItem
    {
        public override T GetItem<T>(IItemCreator<T> t)
        {
            return t.GetItem(this);
        }


        //Actuators are used to display decorations on walls and floors and to implement the dungeon mechanics.
        //There are many type of actuators.For some types, the effect of the actuator is different whether the actuator is on a floor tile or a wall tile.
        //Actuators can target a tile that can be any tile on the same map ('Remote' type) or the tile where the actuator itself is located('local' type).
        //Actuators can target other actuators, doors and pits(to open and close them), teleporters and trick walls(to enable or disable them) and texts.

        //    1 word: Next object ID.
        //parent//public int NextObjectID { get; set; }

        
        //    1 word:
        //        Bits 15-7: Data (content depends on actuator type, see below)
        public int Data { get; set; }


        //        Bits 6-0: Type(all actuator types are listed below)
        public int AcutorType { get; set; }

        //    1 word:
        //        Bits 15-12: Graphic decoration ordinal in the list of allowed decorations(0 means no decoration). If there are several actuators in the same location, only the last one in the list is displayed and determines the active screen zones for mouse clicking.
        public int Decoration { get; set; }

        //       Bits 11: Action target type
        //            '0' Remote tile
        //            '1' Local tile
        public bool IsLocal { get; set; }

        //        Bits 10-7: Delay before action
        public int ActionDelay { get; set; }

        //       Bits 6: Sound effect
        public bool HasSoundEffect { get; set; }
        
        //        Bits 5: Revert effect when stepping in and out of a tile
        public bool IsRevertable { get; set; }
        
        //       Bits 4-3: Action type
        //            '00' Set
        //            '01' Clear
        //            '10' Toggle (set if cleared, clear if set)
        //            '11' Hold
        public ActionType Action { get; set; }

        //        Bits 2: Once only actuator.When this bit is set, the actuator is disabled after its first activation by changing its type to 0.
        public bool IsOnceOnly { get; set; }

        //        Bits 1-0: Unused


        //    1 word: This word has different meaning depending on the action target type (Remote or Local).
        public Target ActLoc { get; set; }

        //Notes:

        //    Bug: if you put creatures and actuators on the same tile, the actuators must be located before the creature in the list of objects or the game will crash.
        //    Bug: Actuators should not be put on the same wall as a champion portrait or the champion portrait will display above the actuator graphic. However, you can have a text object on the same tile with no issue.
        //    Only tiles of type 'Wall' can have wall actuators. All other tile types are considered as floor regarding actuators.
        //    If you have an actuator on a strairs tile, it is only activated when you arrive by this stair, not when you leave to another level (maybe that was the purpose of the type 5 actuator, but it does not work).
        //    It is possible to have actuators on trick walls, but only without decoration or the game may crash.Activation is similar to a floor tile.

        //You can refer to the Technical Documentation - Dungeon Master and Chaos Strikes Back Items properties page to find the complete list of index values to use in the Data field for each item.
        //Floor actuators
        //Actuator type Details
        //00 (Floor)
        //Inactive floor actuator Activated by: Nothing
        //Example use: display a floor decoration
        //Note: Once activated, the type of wall actuators that have the 'once only' flag is set to 'Inactive floor actuator'.
        //01 (Floor)
        //Floor actuator TPCI Activated by: Theron, Party, Creatures, Items
        //02 (Floor)
        //Floor actuator TPC Activated by: Theron, Party, Creatures(but not items)
        //03 (Floor)
        //Floor actuator P Activated by: Party(but not Theron, Creatures and items). Activation also depends on the data value:

        //    '0' Always activated
        //    '1' Activated only if party faces north
        //    '2' Activated only if party faces east
        //    '3' Activated only if party faces south
        //    '4' Activated only if party faces west

        //04 (Floor)
        //Floor actuator I Activated by: The item specified in the Data field(but not Theron, Party or Creatures). The effect is triggered when at least one item of this type is placed on the tile and canceled when all of them are removed.
        //05 (Floor)
        //Stairs actuator Activated by: Party when it arrives from stairs. This type of actuator can only be put on stairs tiles.
        //Does not work before version 2.1 because of a bug in the code.
        //06 (Floor)
        //Creature generator Activated by: Another actuator (via a timer object of type 5).
        //Generates a creature group on the tile.If the tile is occupied, for example by another creature group, the timer stays in the queue for later processing until the tile is free.
        //The two other words are used differently than for other actuators:
        //1 word:

        //    Bits 15-12: Graphic number
        //    Bit 11: Unused
        //    Bit 10: Number of creatures
        //        '0' Predefined number of generated creatures, stored in bits 9-7
        //        '1' Random number of generated creatures, their maximum number is stored in bits 9-7
        //    Bits 9-7: Value for the number of creatures.Allowed values: 1 à 4, others values can cause crashes.
        //    Bit 6: Sound effect (teleport sound)
        //    Bits 5-3: Unused
        //    Bit 2: Once only actuator
        //    Bits 1-0: Unused

        //1 word:

        //    Bits 15-8: Delay used for a Timer of type 65 (Such a timer changes the first actuator of type 0 to type 6 on the tile). If the delay is 0 no timer is created.Values 1 to 127 cause the timer to expire 1 to 127 clock ticks later.Values 128 to 255 cause the timer to expire 64 x(Value - 126) clock ticks later.
        // Bits 7-4: Hit point multiplier.If set to 0, the experience multiplier of the map is used.
        //Bits 3-0: Unused

        //07 (Floor)
        //Floor actuator C Activated by: Creatures(but not by Theron, Party or Items)
        //08 (Floor)
        //Party possession detector Activated by the Party if any champion has the item specified in the Data field in its possessions(including the item in hand).
        //09 (Floor)
        //Version checker     Implemented in engine version 2.0 and above of Dungeon Master and Chaos Strikes Back(no effect in earlier versions).
        //Put the required version in the Data field as 10 x version + revision.Example: '21' for version 2.1.
        //Activated if game engine version <= required version.
        //Activated by Theron and Party only.
        //Meynaf has created a custom dungeon that can detect the version of the game using this actuator: Version checker dungeon
        //Wall actuators
        //Actuator type Details
        //00 (Wall)
        //Inactive wall actuator Activated by: Nothing
        //Example use: display a wall decoration
        //Note: Once activated, the type of wall actuators that have the 'once only' flag is set to 'Inactive wall actuator'.
        //01(Wall)   Activated by: Mouse click on decoration with empty hand or any item.
        //For alcoves: any item placed in the alcove.
        //02(Wall)   Activated by: If the 'Revert effect' flag is not set: mouse click on decoration with any item but not with empty hand.
        //If the 'Revert effect' flag is set: mouse click on decoration with empty hand but not with any item. For alcoves: can only be activated with the idem specified in the data field.
        //03(Wall)   Activated by: If the 'Revert effect' flag is not set: mouse click on decoration with the item specified in the data field but not with any other item or empty hand.
        //If the 'Revert effect' flag is set: mouse click on decoration with any item but not with the item specified in the data field.
        //For alcoves: Any item except the one specified in the data field.
        //The item used to activate the actuator is not removed from the hand.
        //04(Wall)   Activated by: If the 'Revert effect' flag is not set: mouse click on decoration with the item specified in the data field but not with any other item or empty hand.
        //If the 'Revert effect' flag is set: mouse click on decoration with any item but not with the item specified in the data field or empty hand.
        //The item used to activate the actuator is removed from the hand.
        //For alcoves: cannot be activated.
        //05(Wall)
        //And / Or gate Activated by: Another actuator. (A type 6 timer is used to set/ clear / toggle the bit specified by byte 8(position) of the timer object)
        //The data field contains two 4 bits values. A reference state that is defined at design time, and a current state that can be changed through activations. The direction of an activation determines which bit of the current state is affected.

        //    Bit 15: Unused
        //    Bit 14: Reference state, West
        //    Bit 13: Reference state, South
        //    Bit 12: Reference state, East
        //    Bit 11: Reference state, North
        //    Bit 10: Current state, West
        //    Bit 9: Current state, South
        //    Bit 8: Current state, East
        //    Bit 7: Current state, North

        //Effect: If the two 4 bits values are identical, the effect is produced.
        //If the two values are identical from the beginning, the actuator still requires at least one activation before the effect is produced.
        //06(Wall)
        //Counter Activated by:
        //    Another actuator
        //Direction of this actuator and direction of activation are ignore.
        //The data field is used as a counter.Each 'open' activation increases the counter, each 'close' or 'toggle' activation decreases the counter. When the counter reaches 0, the effect of the actuator is triggered and the actuator is disabled definitively.
        //The actuator is considered 'pressed' if the counter is non - zero.
        //07(Wall)
        //Weapon missile shooter Activated by:
        //    Another actuator
        //Effect: Shoots a missile in the direction defined by the position of the actuator on the tile. The missile is shot randomly from the left or right.
        //Direction of activation:
        //    Used.
        //Example use: usually a wall decoration is displayed to show holes in the wall.
        //The two other words are used differently than for other actuators:
        //1 word:

        //    Bits 15 - 12: Graphic number
        //    Bit 11: Always set to 1 but has no effect.
        //    Bit 2: Once only actuator

        //1 word:

        //    Bits 15 - 12: Missile power decrease.The higher this value, the quicker the power will decrease and the shorter will be the shot.The missile power is decreased by this value at each clock tick.This only works correctly starting with Chaos Strikes Back v2.1.In previous versions this value is hard coded to 0.
        //    Bits 11 - 4: Missile power.Determines the size of magic missiles and damage.

        //The data field defines the missile type(items are created, no supply needed):

        //    '4' to '7': Torch
        //    '32' Dagger
        //    '51' Arrow
        //    '52' Slayer
        //    '54' Rock
        //    '55' Poison dart
        //    '56' Throwing star
        //    '128' Boulder

        //08(Wall)
        //Magic missile shooter   Identical to type 7, but a magic missile is created based on the value of the Data field:

        //    '0' Fireball
        //    '1' Poison Blob
        //    '2' Lightning Bolt
        //    '3' Dispell
        //    '4' Zo spell
        //    '6' Poison Bolt
        //    '7' Poison Cloud

        //09(Wall)
        //Double weapon missile shooter   Identical to type 7(Wall), but shoots two missiles(one on each side)
        //10(Wall)
        //Double magic missile shooter    Identical to type 8(Wall), but shoots two missiles(one on each side)
        //11(Wall)   Identical to type 4 with these two differences:

        //    the actuator is moved to the beginning of the list when activated
        //    the actuator can only be activated when it is at the end of the list

        //12(Wall)
        //Item creator    Activated by: Mouse click with empty hand
        //Effect: Creates an item of type specified in the Data field and puts it in the hand.
        //The actuator is moved to the beginning of the list when activated.
        //The actuator can only be activated when it is at the end of the list.
        //Can also activate another actuator.
        //13(Wall)
        //Item storage    Activated by: Mouse click with empty hand or Mouse click with an item of type specified in the Data field.
        //Effect: When clicked with empty hand, an item of the type specified in the Data field is put in the hand if one can be found on the actuator tile(no matter which position on the tile).If no item is available on the tile there is no effect.When clicked with an item of the type specified in the Data field in hand, the item is removed from the hand and placed in the wall.
        //Example use: a torch holder.You can put a torch and get it from the torch holder.
        //Can also activate another actuator.
        //14(Wall)
        //Item missile shooter Activated by: Another actuator (by a type 6 timer)
        //Effect: Similar to type 7 but shoots the first item found on the actuator tile itself, on the same position as the actuator.
        //When there are no more items to shoot, the game can crash or shoot magic missiles.This bug was fixed in CSBwin.
        //15(Wall)
        //Double item missile shooter     Similar to type 14 but shoots the first two items. This actuator has the same bug when there are no more items to shoot.
        //In Chaos Strikes Back, the "Give up - pull the lever" zone uses this actuator type and works fine only by chance (it crashes in Dungeon Master v1.2)
        //16(Wall)
        //Item exchanger  Activated by: Mouse click with the item specified in the Data field in hand.The item is exchanged with the first item found on the actuator tile(its position on the tile is ignored).If no item is found on the tile, activation does not occur.
        //17(Wall)   Identical to type 11 but if it is not the only actuator on its wall side, the actuator is removed from the list.
        //18(Wall)
        //End game    Activated by: Another actuator with any action and any direction.
        //Effect: triggers the end of the game (simply displays portraits on ST versions). Only works with Chaos Strikes Back v2.0 and above.
        //Starting with version 2.1, a delay in seconds can be configured in Bits 10 - 7 of the second word. The game is stopped during this delay.The delay is hard coded in version 2.0.The delay is removed in versions with an end sequence like Chaos Strikes Back for Amiga v3.3.
        //Note : the Chaos Strikes Back dungeon does not use this delay and blocks the party with teleporters.
        //Bug : the delay value is always multiplied by 60 to get a number of VBL to wait.This works fine for an NTSC system but for a PAL system the delay value should be multiplied by 50.The effect is that on PAL system the delay seems to be multiplied by 1, 2 seconds.
        //127(Wall)
        //Champion portrait(Dungeon Master and Chaos Strikes Back)   The Data field contains the champion graphic number to use(0 to 23).Here is the list of c-hampions to use:

        //   00 Elija / Airwing
        //    01 Halk / Aroc
        //    02 Syra / Talon
        //    03 Hissssa / Leta
        //    04 Zed / Dema
        //    05 Chani / Algor
        //    06 Hawk / Toadrot
        //    07 Boris / Ven
        //    08 Mophus / Mantia
        //    09 Leif / Gnatu
        //    10 Wu Tse / Slogar
        //    11 Alex / Sting
        //    12 Linflas / Skelar
        //    13 Azizi / Deth
        //    14 Iaido / Necro
        //    15 Gando / Plague
        //    16 Stamm / Tunda
        //    17 Leyla / Lana
        //    18 Tiggy / Buzzzz
        //    19 Sonja / Petal
        //    20 Nabi / Itza
        //    21 Gothmog / Tula
        //    22 Wuuf / Kazai
        //    23 Daroou / Lor

        //When the champion has been resurrected or reincarnated, the actuator is disabled (changed to type 0).However, if other actuators are present on the same tile, a bug allows cloning of the champion.
        //The tile in front of the portrait must contain a text object (preferably disabled) containing the champion statistics and skills. The game crashes if this text object is missing.
        //Note: although in FTL dungeons these actuators always target the tile in front of the portrait with a direction identical to that of the portrait, the game ignores this and always uses the tile in front of the portrait, no matter which tile is specified.
        //Other actuators
        //Actuator type Details
        //19 to 125
        //Unused in Dungeon Master and Chaos Strikes Back. Some are used in Dungeon Master II) 	-
        //126
        //Champion portrait (Dungeon Master II only) 	-

        //All of the Dungeon Master and Chaos Strikes Back actuators can also be used in Dungeon Master II except the champion portrait actuator which is not recognized.Dungeon Master II also includes many additional actuator types.Please visit Kentaro.k - 21's web site for more information about them.
        
    }
}