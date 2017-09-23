using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.GrabableItems;
using DungeonMasterEngine.DungeonContent.GrabableItems.Misc;
using DungeonMasterEngine.DungeonContent.Tiles.Support;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class ViAltairAlcove : Alcove
    {
        public ViAltairAlcove(IEnumerable<IGrabableItem> items) : base(items) { }

        public override bool Trigger(ILeader leader)
        {
            var bones = leader.Hand as ChampionBones;
            if (bones != null && leader.PartyGroup.Count < 4)
            {
                bones.Champion.Rebirth();
                if (!leader.AddChampoinToGroup(bones.Champion))
                    throw new InvalidOperationException();

                leader.Hand = null;
                return true;
            }
            else
            {
                return base.Trigger(leader);
            }
        }
    }
}