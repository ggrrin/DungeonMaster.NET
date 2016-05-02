using System;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Actuators
{
    public abstract class SimpleRemoteActuator : RemoteActuator
    {
        public Tile TargetTile { get; }

        public ActionStateX TargetAction { get; }

        protected SimpleRemoteActuator(Tile targetTile, ActionStateX targetAction, Vector3 position) : base(position)
        {
            if (targetTile == null || targetAction == null)
                throw new ArgumentNullException();

            TargetTile = targetTile;
            TargetAction = targetAction;

        }

        protected virtual async void SendMessageAsync(bool activated)
        {
            await SendOneMessageAsync(TargetTile, TargetAction, activated);
        }

        protected sealed override void PerformMessage(Tile targetTile, ActionStateX action, bool activated)
        {
            base.PerformMessage(targetTile, action, activated);
        }
    }
}