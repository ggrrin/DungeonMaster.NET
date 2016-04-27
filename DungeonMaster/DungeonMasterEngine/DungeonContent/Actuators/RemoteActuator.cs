using System;
using System.Threading.Tasks;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public abstract class RemoteActuator : Actuator
    {
        public bool Activated { get; protected set; } = true;

        public Tile TargetTile { get; }

        public abstract ActionStateX TargetAction { get; }

        protected RemoteActuator(Tile targetTile, Vector3 position) : base(position)
        {
            if (targetTile == null)
                throw new ArgumentException();

            TargetTile = targetTile;
        }

        protected virtual async void SendMessageAsync()
        {
            await SendOneMessageAsync(TargetAction);
        }

        protected async Task SendOneMessageAsync(ActionStateX action)
        {
            if (!action.IsOnceOnly || !action.Used)
            {
                await Task.Delay(action.TimeDelay);
                PerformMessage(action);
                action.Used = true;
            }
        }

        protected virtual void PerformMessage(ActionStateX action)
        {
            TargetTile.ExecuteContentActivator(new LogicTileActivator(action));
            switch (action.Action)
            {
                case ActionState.Clear:
                    if (Activated)
                        TargetTile.DeactivateTileContent();
                    break;
                case ActionState.Set:
                    if (Activated)
                        TargetTile.ActivateTileContent();
                    break;
                case ActionState.Toggle:
                    if (Activated)
                        Toggle();
                    break;
                case ActionState.Hold:
                    Toggle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void Toggle(bool invertEffect = false)
        {
            if (TargetTile.ContentActivated ^ invertEffect)
                TargetTile.DeactivateTileContent();
            else
                TargetTile.ActivateTileContent();
        }
    }
}