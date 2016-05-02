using System;
using DungeonMasterEngine.Helpers;
using DungeonMasterParser.Enums;
using DungeonMasterParser.Items;
using DungeonMasterParser.Support;

namespace DungeonMasterEngine.Builders
{
    public class ActuatorState : IEquatable<ActuatorItemData>
    {
        //nullable types are optional if it is null
        public ActionType? Action { get; set; }
        public int ActuatorType { get; set; }
        public bool? RotateActuator { get; set; }
        public bool? IsLocal { get; set; }

        public bool Equals(ActuatorItemData i)
        {
            if (i == null)
                throw new ArgumentNullException();

            return ActuatorType == i.ActuatorType && IsLocal.OptionalyEquals(i.IsLocal) && RotateActuator.OptionalyEquals((i.ActionLocation as LocTrg)?.RotateAutors);
        }
    }
}