using System.Linq;
using DungeonMasterEngine.DungeonContent.Items;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.Interfaces;
using DungeonMasterEngine.Player;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public class PartyPossesionActuator : FloorActuator 
    {
        public PartyPossesionActuator(Vector3 position, Tile actuatorTile, Tile targetTile, IConstrain constrain, ActionStateX action) : 
            base(position, actuatorTile, targetTile, constrain,  action)
        { }

        protected override void TestAndRun(object enteringObject)
        {
            var theron = enteringObject as Theron;
            if (theron == null)
            {
                var items = new[] { theron.Hand }
                    .Concat(theron.PartyGroup.SelectMany(x => x.Inventory))
                    .Concat(CurrentTile.SubItems);

                if (items.Any(Constrain.IsAcceptable))
                    SendMessage();
            }
        }
    }
}
