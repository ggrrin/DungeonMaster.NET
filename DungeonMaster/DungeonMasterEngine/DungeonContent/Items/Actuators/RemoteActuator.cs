using System;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Items.Actuators
{
    public class RemoteActuator : Actuator
    {
        public Tile TargetTile { get; }

        public ActionStateX TargetAction { get; }

        public RemoteActuator(Tile targetTile, ActionStateX action, Vector3 position) : base(position)
        {
            TargetTile = targetTile;
            TargetAction = action;
        }

        protected void SendMessage(bool actuatorActivated = false)
        {
            TargetTile.ExecuteContentActivator(new LogicTileActivator(TargetAction));

            switch (TargetAction.Action)
            {
                case ActionState.Clear:
                    TargetTile.DeactivateTileContent();
                    break;
                case ActionState.Set:
                    TargetTile.ActivateTileContent();
                    break;
                case ActionState.Toggle:
                    if (TargetTile.ContentActivated)
                        TargetTile.DeactivateTileContent();
                    else
                        TargetTile.ActivateTileContent();
                    break;
                case ActionState.Hold:
                    if (actuatorActivated)
                        TargetTile.ActivateTileContent();
                    else
                        TargetTile.DeactivateTileContent();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}