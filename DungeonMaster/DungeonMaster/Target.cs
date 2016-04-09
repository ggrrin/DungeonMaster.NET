using DungeonMasterParser.Structures;

namespace DungeonMasterParser
{
    public abstract class Target { }

    public class RemoteTarget :Target
    {
        //    For remote target:
        //        Bits 15-11: Y coordinate of target tile
        //        Bits 10-6: X coordinate of target tile
        //        Bits 5-4: Direction (for a wall tile, determines which wall's face is triggered, North East South West)
        //        Bits 3-0: Unused

        public MapPosition Position { get; set; }


        public override string ToString()
        {
            return Position.ToString();
        }

        public int Direction => (int)Position.Direction;
    }

    public class LocalTarget : Target
    {
        //    For local target:
        //        Bits 15-4: Action to execute
        //            '0' No action
        //            '1' or '2' Rotation of actuators in the same tile. Sends the current actuator to the beginning of the list.
        public bool RotateAutors { get; set; }
            
            //            '10' Experience gain in Ninja skill number 8. If the actuator is on a floor tile, all champions get the experience. If the actuator is on a wall tile, only the party leader gets the experience.

        public bool ExperienceGain { get; set; }
        //            Other values: No action
        //        Bits 3-0: Unused
    }
}