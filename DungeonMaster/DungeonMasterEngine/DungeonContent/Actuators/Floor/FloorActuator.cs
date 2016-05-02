using System;
using System.Collections.Generic;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public class FloorActuator : RemoteActuator
    {
        private readonly IEnumerable<Tile> targetTile;
        private readonly IEnumerable<ActionStateX> action;

        public Tile CurrentTile { get; }


        public bool Enabled { get; private set; } = true;

        public virtual IConstrain Constrain { get; }

        public FloorActuator(Vector3 position, Tile currentTile, IConstrain constrain, IEnumerable<Tile> targetTile, IEnumerable<ActionStateX> action) : base(position)
        {
            this.targetTile = targetTile.ToArray();
            this.action = action.ToArray();
            Activated = false;
            CurrentTile = currentTile;
            Constrain = constrain;

            currentTile.ObjectEntered += CurrentTile_ObjectEntered;
            currentTile.ObjectLeft += CurrentTile_ObjectLeft;
        }

        private void CurrentTile_ObjectEntered(object sender, object e)
        {
            if (Enabled && !Activated)
            {
                if (TriggerCondition())
                {
                    Activated = true;
                    SendMessageAsync(Activated);
                }
            }
        }

        private async void SendMessageAsync(bool activated)
        {
            await Task.WhenAll(targetTile.Zip(action, (x,y) => SendOneMessageAsync(x,y,activated)));
        }

        private void CurrentTile_ObjectLeft(object sender, object e)
        {
            if (Enabled && Activated)
            {
                if (!TriggerCondition()) //hold message
                {
                    Activated = false;
                    SendMessageAsync(Activated);
                }
            }
        }

        protected virtual bool TriggerCondition() => CurrentTile.SubItems.Any(Constrain.IsAcceptable);

        public override string ToString()
        {
            return $"{base.ToString()} {GetType().Name} actions: {string.Join(Environment.NewLine, action)}\r\n {Constrain}";
        }
    }
}

