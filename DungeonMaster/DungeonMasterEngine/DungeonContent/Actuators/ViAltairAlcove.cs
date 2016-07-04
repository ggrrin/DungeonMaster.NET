using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Entity.Properties;
using DungeonMasterEngine.DungeonContent.Entity.Properties.Base;
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
            if (bones != null)
            {
                var health = bones.Champion.GetProperty(PropertyFactory<HealthProperty>.Instance);
                health.Value = health.MaxValue;
                if (leader.AddChampoinToGroup(bones.Champion))
                {
                    leader.Hand = null;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return base.Trigger(leader);
            }
        }
    }
}