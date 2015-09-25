namespace DungeonMasterParser
{
    public enum DoorState
    {
        //'000' Open
        //'001' 1 / 4 closed
        //'010' 1 / 2 closed
        //'011' 3 / 4 closed
        //'100' Closed
        //'101' Bashed
        Open = 0, QuarterClosed, HalfClose,  ThreeQuartersClosed, Closed, Bashed
    }
}