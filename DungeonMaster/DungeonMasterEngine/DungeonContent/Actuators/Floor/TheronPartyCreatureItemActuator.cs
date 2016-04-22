using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public class TheronPartyCreatureItemActuator : FloorActuator
    {
        public TheronPartyCreatureItemActuator(Vector3 position, Tile currentTile, Tile targetTile, ActionStateX action) : base(position, currentTile, targetTile, action)
        {}

        
        protected override void TestAndRun(object e, bool objectEnter)
        {
            bool activated = e is Theron || e is Creature || e is GrabableItem;//TODO party test not only Theron 1!!!!

            if (activated)
                Activate();
        }

        protected virtual void Activate()
        {
            AffectTile();
        }


    }
}
