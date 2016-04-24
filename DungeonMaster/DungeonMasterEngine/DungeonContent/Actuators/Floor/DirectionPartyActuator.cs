using System;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public class PartDirectionConstrain : IConstrain
    {
        public MapDirection AcceptDirection { get; }

        public PartDirectionConstrain(MapDirection acceptDirection)
        {
            AcceptDirection = acceptDirection;
        }

        public bool IsAcceptable(object item)
        {
            var theron = item as Theron;
            return theron?.PartyGroup.Count == 4 && theron.MapDirection == AcceptDirection;
        }
    }

    public enum MapDirection
    {
        North = 0, East, South, West
    }
}
