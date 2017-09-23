using System;

namespace DungeonMasterEngine.Graphics
{
    [Flags]
    public enum CubeFaces : byte
    {
        None = 0, Back = 1, Right = 2, Front = 4, Left = 8, Floor = 16, Ceeling = 32,
        Sides = 0x0F, Horizontal = 48, All = 0x3F
    }
}