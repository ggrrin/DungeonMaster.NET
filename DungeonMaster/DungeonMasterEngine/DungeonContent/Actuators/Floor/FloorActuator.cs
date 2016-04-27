using System;
using DungeonMasterEngine.DungeonContent.Tiles;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Security.Policy;
using DungeonMasterEngine.Interfaces;

namespace DungeonMasterEngine.DungeonContent.Actuators.Floor
{
    public class FloorActuator : RemoteActuator
    {

        public Tile CurrentTile { get; }

        public override ActionStateX TargetAction { get; }

        public bool Enabled { get; private set; } = true;

        public virtual IConstrain Constrain { get; }

        public FloorActuator(Vector3 position, Tile currentTile, Tile targetTile, IConstrain constrain, ActionStateX action) : base(targetTile, position)
        {
            Activated = false;
            CurrentTile = currentTile;
            TargetAction = action;
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
                    SendMessageAsync();
                }
            }
        }

        private void CurrentTile_ObjectLeft(object sender, object e)
        {
            if (Enabled && Activated)
            {
                if (!TriggerCondition()) //hold message
                {
                    Activated = false;
                    SendMessageAsync();
                }
            }
        }

        protected virtual bool TriggerCondition() => CurrentTile.SubItems.Any(Constrain.IsAcceptable);

        public override string ToString()
        {
            return $"{base.ToString()} {GetType().Name} action: {TargetAction}\r\n {Constrain}";
        }
    }
}

