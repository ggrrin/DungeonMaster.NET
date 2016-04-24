using System;
using DungeonMasterEngine.DungeonContent.Actuators.Wall;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public abstract class RemoteActuator : Actuator
    {
        public virtual bool Activated { get; protected set; } = false;
        public Tile TargetTile { get; }

        public abstract ActionStateX TargetAction { get; }

        public RemoteActuator(Tile targetTile, Vector3 position) : base(position)
        {
            TargetTile = targetTile;
        }

        protected void SendMessage()
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
                    if (Activated)
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