namespace DungeonMasterParser
{
    public enum ActionType
    {
        //       Bits 4-3: Action type
        //            '00' Set
        //            '01' Clear
        //            '10' Toggle (set if cleared, clear if set)
        //            '11' Hold

        Set = 0, Clear, Toggle, Hold
    }
}