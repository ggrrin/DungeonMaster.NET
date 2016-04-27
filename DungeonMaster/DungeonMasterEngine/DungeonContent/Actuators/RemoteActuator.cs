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

        public bool Used { get; private set; } = false;

        public Tile TargetTile { get; }

        public abstract ActionStateX TargetAction { get; }

        protected RemoteActuator(Tile targetTile, Vector3 position) : base(position)
        {
            if(targetTile == null)
                throw new ArgumentException();

            TargetTile = targetTile;
        }

        protected async void SendMessageAsync()
        {
            if (TargetAction.IsOnceOnly && Used)
                return;


            await Task.Delay(TargetAction.TimeDelay);
            PerformMessage();
            Used = true;
        }


        protected virtual void PerformMessage()
        {
            TargetTile.ExecuteContentActivator(new LogicTileActivator(TargetAction));
            switch (TargetAction.Action)
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