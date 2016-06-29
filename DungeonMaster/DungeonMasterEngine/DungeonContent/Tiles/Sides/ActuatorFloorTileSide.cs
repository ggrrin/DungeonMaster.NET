using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Actuators;
using DungeonMasterEngine.DungeonContent.GrabableItems;

namespace DungeonMasterEngine.DungeonContent.Tiles.Sides
{
    public class ActuatorFloorTileSide : FloorTileSide
    {
        public FloorActuator Actuator { get; }

        public override void OnObjectEntered(object localizable, bool addToSpacd = true)
        {
            Actuator.Trigger(localizable, subItems, true);
            base.OnObjectEntered(localizable, addToSpacd);
        }

        public override void OnObjectLeft(object localizable)
        {
            base.OnObjectLeft(localizable);
            Actuator.Trigger(localizable, subItems, false);
        }

        public ActuatorFloorTileSide(FloorActuator actuator, bool randomDecoration, MapDirection face, IEnumerable<IGrabableItem> topLeftItems, IEnumerable<IGrabableItem> topRightItems, IEnumerable<IGrabableItem> bottomLeftItems, IEnumerable<IGrabableItem> bottomRightItems) : base(randomDecoration, face, topLeftItems, topRightItems, bottomLeftItems, bottomRightItems)
        {
            Actuator = actuator;
        }
    }
}