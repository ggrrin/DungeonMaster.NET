namespace DungeonMasterParser.Support
{
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