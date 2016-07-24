using System.Linq;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent;
using DungeonMasterEngine.DungeonContent.Tiles;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.GameConsoleContent.Base;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.GameConsoleContent
{
    public class TeleportCommand : Interpreter
    {
        public override async Task Run()
        {
            Point targetLocation;
            if (Parameters.Length == 2 && int.TryParse(Parameters[0], out targetLocation.X) && int.TryParse(Parameters[1], out targetLocation.Y))
            {
                var theron = ConsoleContext.AppContext.Leader;
                int currentLevel = theron.Location.Tile.LevelIndex;
                var level = ConsoleContext.AppContext.ActiveLevels.First(x => x.LevelIndex == currentLevel);//Level has to be there 
                Tile targetTile = null;
                level.TilesPositions.TryGetValue(targetLocation, out targetTile);

                if (targetTile != null && targetTile.IsAccessible)
                {
                    theron.Location = theron.Location.GetNew(targetTile);
                }
                else
                {
                    Output.WriteLine("Invalid target location.");
                }
            }
            else if (Parameters.Length == 3 && Parameters[2] == "x" && int.TryParse(Parameters[0], out targetLocation.X) && int.TryParse(Parameters[1], out targetLocation.Y))
            {
                var target = ConsoleContext.AppContext.Leader.Location.Tile.Level.TilesPositions[targetLocation];
                target.AcceptMessageBase(new Message (MessageAction.Toggle, MapDirection.North));
            }
            else
            {
                Output.WriteLine("Invalid parameters");
            }
        }
    }
}
