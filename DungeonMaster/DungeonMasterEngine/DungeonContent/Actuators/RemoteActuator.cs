using System;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public class RemoteActuator : Actuator
    {
        public bool Activated { get; protected set; } = true;

        public RemoteActuator(Vector3 position) { } 
        
        protected async Task SendOneMessageAsync(Tile targetTile, ActionStateX action, bool activated)
        {
            if (!action.IsOnceOnly || !action.Used)
            {
                await Task.Delay(action.TimeDelay);
                PerformMessage(targetTile, action, activated);
                action.Used = true;
            }
        }

        protected virtual void PerformMessage(Tile targetTile, ActionStateX action, bool activated)
        {
            targetTile.ExecuteContentActivator(new LogicTileActivator(action));
            switch (action.Action)
            {
                case ActionState.Clear:
                    if (activated)
                        targetTile.DeactivateTileContent();
                    break;
                case ActionState.Set:
                    if (activated)
                        targetTile.ActivateTileContent();
                    break;
                case ActionState.Toggle:
                    if (activated)
                        Toggle(targetTile);
                    break;
                case ActionState.Hold:
                    Toggle(targetTile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void Toggle(Tile targetTile, bool invertEffect = false)
        {
            if (targetTile.ContentActivated ^ invertEffect)
                targetTile.DeactivateTileContent();
            else
                targetTile.ActivateTileContent();
        }
    }
}